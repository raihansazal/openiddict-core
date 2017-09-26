﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/openiddict/openiddict-core for more information concerning
 * the license and the contributors participating to this project.
 */

using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using OpenIddict.Models;

namespace OpenIddict.EntityFramework
{
    /// <summary>
    /// Provides methods allowing to manage the scopes stored in a database.
    /// Note: this class can only be used with the default OpenIddict entities.
    /// </summary>
    /// <typeparam name="TContext">The type of the Entity Framework database context.</typeparam>
    public class OpenIddictScopeStore<TContext> : OpenIddictScopeStore<OpenIddictScope, TContext, string>
        where TContext : DbContext
    {
        public OpenIddictScopeStore([NotNull] TContext context) : base(context) { }
    }

    /// <summary>
    /// Provides methods allowing to manage the scopes stored in a database.
    /// Note: this class can only be used with the default OpenIddict entities.
    /// </summary>
    /// <typeparam name="TContext">The type of the Entity Framework database context.</typeparam>
    /// <typeparam name="TKey">The type of the entity primary keys.</typeparam>
    public class OpenIddictScopeStore<TContext, TKey> : OpenIddictScopeStore<OpenIddictScope<TKey>, TContext, TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
    {
        public OpenIddictScopeStore([NotNull] TContext context) : base(context) { }
    }

    /// <summary>
    /// Provides methods allowing to manage the scopes stored in a database.
    /// Note: this class can only be used with the default OpenIddict entities.
    /// </summary>
    /// <typeparam name="TScope">The type of the Scope entity.</typeparam>
    /// <typeparam name="TContext">The type of the Entity Framework database context.</typeparam>
    /// <typeparam name="TKey">The type of the entity primary keys.</typeparam>
    public class OpenIddictScopeStore<TScope, TContext, TKey> : Core.OpenIddictScopeStore<TScope, TKey>
        where TScope : OpenIddictScope<TKey>, new()
        where TContext : DbContext
        where TKey : IEquatable<TKey>
    {
        public OpenIddictScopeStore([NotNull] TContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Context = context;
        }

        /// <summary>
        /// Gets the database context associated with the current store.
        /// </summary>
        protected virtual TContext Context { get; }

        /// <summary>
        /// Gets the database set corresponding to the <typeparamref name="TScope"/> entity.
        /// </summary>
        protected DbSet<TScope> Scopes => Context.Set<TScope>();

        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns the single element returned when executing the specified query.
        /// </returns>
        public override Task<TResult> GetAsync<TResult>([NotNull] Func<IQueryable<TScope>, IQueryable<TResult>> query, CancellationToken cancellationToken)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return query.Invoke(Scopes).SingleOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to abort the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to monitor the asynchronous operation,
        /// whose result returns all the elements returned when executing the specified query.
        /// </returns>
        public override Task<TResult[]> ListAsync<TResult>([NotNull] Func<IQueryable<TScope>, IQueryable<TResult>> query, CancellationToken cancellationToken)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return query.Invoke(Scopes).ToArrayAsync(cancellationToken);
        }
    }
}