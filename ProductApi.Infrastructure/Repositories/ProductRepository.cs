﻿using eCommerce.SharedLibrary.Logs;
using eCommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext context) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                // check if product already exists
                var getProduct = await GetByAsync(_ => _.Name.Equals(entity.Name));

                if (getProduct is not null && !string.IsNullOrEmpty(getProduct.Name))
                    return new Response(false, $"{entity.Name} already added");

                var currentEntity = context.Products.Add(entity).Entity;
                await context.SaveChangesAsync();

                if (currentEntity is not null && currentEntity.Id > 0)
                    return new Response(true, $"{entity.Name} added to database successfully");
                else
                    return new Response(false, $"Error occured while adding {entity.Name}");

            }
            catch (Exception ex)
            {
                // log original exception
                LogException.LogExceptions(ex);

                // display scare-free messages to the client
                return new Response(false, "Error while adding new product");
            }
        }

        public async Task<Response> DeleteAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if (product is null)
                    return new Response(false, $"{entity.Name} not found");

                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} deleted successfully");
            }
            catch (Exception ex)
            {
                // log original exception
                LogException.LogExceptions(ex);

                // display scare-free messages to the client
                return new Response(false, "Error while deleting product");
            }
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);

                return product is not null ? product : null!;
            }
            catch (Exception ex)
            {
                // log original exception
                LogException.LogExceptions(ex);

                // display scare-free messages to the client
                throw new Exception("Error while retrieving product");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await context.Products.AsNoTracking().ToListAsync();
                return products is not null ? products : null!;
            }
            catch (Exception ex)
            {
                // log original exception
                LogException.LogExceptions(ex);

                // display scare-free messages to the client
                throw new Exception("Error occured retrieving products");
            }

        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var product = await context.Products.Where(predicate).FirstOrDefaultAsync();
                return product is not null ? product : null!;
            }
            catch (Exception ex)
            {
                // log original exception
                LogException.LogExceptions(ex);

                // display scare-free messages to the client
                throw new InvalidOperationException("Error occured retrieving product");
            }
        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if(product is null)
                    return new Response(false, $"{entity.Name} not found");

                context.Entry(product).State = EntityState.Detached;
                context.Products.Update(entity);
                await context.SaveChangesAsync();

                return new Response(true, $"{entity.Name} updated successfully");
            }
            catch (Exception ex)
            {
                // log original exception
                LogException.LogExceptions(ex);

                // display scare-free messages to the client
                return new Response(false,"Error while updating existing product");
            }
        }
    }
}
