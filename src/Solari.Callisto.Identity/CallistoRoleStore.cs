using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Framework;


namespace Solari.Callisto.Identity
{
    // COPIED FROM https://github.com/matteofabbri/AspNetCore.Identity.Mongo
    public class CallistoRoleStore<TRole> : CallistoCollection<TRole>,
                                            IRoleClaimStore<TRole>,
                                            IQueryableRoleStore<TRole>
        where TRole : CallistoIdentityRole, IDocumentRoot


    {
        public CallistoRoleStore(ICallistoCollectionContext<TRole> collectionContext,
                                 ICollectionOperators<TRole> operators)
            : base(collectionContext, operators)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }


        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            TRole found = await Collection.FirstOrDefaultAsync(x => x.NormalizedName == role.NormalizedName,
                                                               cancellationToken)
                                          .ConfigureAwait(false);

            if (found == null)
                await Collection.InsertOneAsync(role, cancellationToken: cancellationToken)
                                .ConfigureAwait(false);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            await Collection.ReplaceOneAsync(x => x.Id == role.Id, role, cancellationToken: cancellationToken)
                            .ConfigureAwait(false);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            await Collection.DeleteOneAsync(x => x.Id == role.Id, cancellationToken).ConfigureAwait(false);

            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.Id.ToString());
        }

        public async Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            return (await Collection.FirstOrDefaultAsync(x => x.Id == role.Id, cancellationToken)
                                    .ConfigureAwait(false))?.Name ?? role.Name;
        }

        public async Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentNullException(nameof(roleName));

            role.Name = roleName;

            await Collection.UpdateOneAsync(x => x.Id == role.Id, Builders<TRole>.Update.Set(x => x.Name, roleName),
                                            cancellationToken: cancellationToken)
                            .ConfigureAwait(false);
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.NormalizedName);
        }

        public async Task SetNormalizedRoleNameAsync(TRole role, string normalizedName,
                                                     CancellationToken cancellationToken)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));
            if (string.IsNullOrEmpty(normalizedName)) throw new ArgumentNullException(nameof(normalizedName));

            role.NormalizedName = normalizedName;

            await Collection.UpdateOneAsync(x => x.Id == role.Id, Builders<TRole>.Update
                                                                                 .Set(x => x.NormalizedName,
                                                                                      normalizedName),
                                            cancellationToken: cancellationToken)
                            .ConfigureAwait(false);
        }

        public Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(roleId)) throw new ArgumentNullException(nameof(roleId));

            return Collection.FirstOrDefaultAsync(x => x.Id == Guid.Parse(roleId), cancellationToken);
        }

        public Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(normalizedRoleName)) throw new ArgumentNullException(nameof(normalizedRoleName));

            return Collection.FirstOrDefaultAsync(x => x.NormalizedName == normalizedRoleName, cancellationToken);
        }

        public async Task<IList<Claim>> GetClaimsAsync(
            TRole role, CancellationToken cancellationToken = new CancellationToken())
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            cancellationToken.ThrowIfCancellationRequested();

            TRole dbRole = await Collection.FirstOrDefaultAsync(x => x.Id == role.Id, cancellationToken)
                                           .ConfigureAwait(false);

            return dbRole.Claims.Select(e => new Claim(e.ClaimType, e.ClaimValue)).ToList();
        }

        public async Task AddClaimAsync(TRole role, Claim claim,
                                        CancellationToken cancellationToken = new CancellationToken())
        {
            if (role == null) throw new ArgumentNullException(nameof(role));
            if (claim == null) throw new ArgumentNullException(nameof(claim));

            cancellationToken.ThrowIfCancellationRequested();

            IdentityRoleClaim<string> currentClaim =
                role.Claims.FirstOrDefault(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);

            if (currentClaim == null)
            {
                var identityRoleClaim = new IdentityRoleClaim<string>()
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                };

                role.Claims.Add(identityRoleClaim);

                await Collection.UpdateOneAsync(x => x.Id == role.Id,
                                                Builders<TRole>.Update.Set(x => x.Claims, role.Claims),
                                                cancellationToken: cancellationToken)
                                .ConfigureAwait(false);
            }
        }

        public async Task RemoveClaimAsync(TRole role, Claim claim,
                                           CancellationToken cancellationToken = new CancellationToken())
        {
            if (role == null) throw new ArgumentNullException(nameof(role));
            if (claim == null) throw new ArgumentNullException(nameof(claim));

            cancellationToken.ThrowIfCancellationRequested();

            role.Claims.RemoveAll(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);

            await Collection.UpdateOneAsync(x => x.Id == role.Id,
                                            Builders<TRole>.Update.Set(x => x.Claims, role.Claims),
                                            cancellationToken: cancellationToken);
        }

        public IQueryable<TRole> Roles => Collection.AsQueryable();
    }
}
