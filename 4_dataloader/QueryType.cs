using GreenDonut;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace HotChocolate.Examples.Paging
{
    public class QueryType
        : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field("messages")
                .Resolver(ctx => ctx.Service<MessageRepository>().GetAllMessages())
                .UsePaging<MessageType>()
                .UseFiltering()
                .Use(next => context => );

            descriptor.Field("usersByCountry")
                .Argument("country", a => a.Type<NonNullType<StringType>>())
                .Type<NonNullType<ListType<NonNullType<UserType>>>>()
                .Resolver(ctx =>
                {
                    var userRepository = ctx.Service<UserRepository>();

                    IDataLoader<string, User[]> userDataLoader =
                        ctx.GroupDataLoader<string, User>(
                            "usersByCountry",
                            userRepository.GetUsersByCountry);

                    return userDataLoader.LoadAsync(ctx.Argument<string>("country"));
                });
        }
    }
}
