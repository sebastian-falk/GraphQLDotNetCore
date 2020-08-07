using GraphQL.DataLoader;
using GraphQL.Types;
using GraphQLDotNetCore.Contracts;
using GraphQLDotNetCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDotNetCore.GraphQL.GraphQLTypes
{
    public class OwnerType : ObjectGraphType<Owner>
    {
        public OwnerType(IAccountRepository accountRepository, IDataLoaderContextAccessor dataLoader)
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Id property from the owner object.");
            Field(x => x.Name).Description("Name property from the owner object.");
            Field(x => x.Address).Description("Address property from the owner object.");
            Field<ListGraphType<AccountType>>(
                "accounts", 
                resolve: context =>
                {
                    var loader = dataLoader
                        .Context
                        .GetOrAddCollectionBatchLoader<Guid, Account>("GetAccountsByOwnerIds", accountRepository.GetAccountsByOwnerIds);
                    return loader.LoadAsync(context.Source.Id);
                });
        }
    }
}
