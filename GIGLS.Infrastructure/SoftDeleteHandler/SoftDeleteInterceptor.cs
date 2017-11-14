using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.Interception;

namespace GIGLS.INFRASTRUCTURE.SoftDeleteHandler
{
    public class SoftDeleteInterceptor : IDbCommandTreeInterceptor
    {
        public void TreeCreated(DbCommandTreeInterceptionContext interceptionContext)
        {
            var dataSpace = interceptionContext.OriginalResult.DataSpace;
            if (dataSpace != DataSpace.SSpace)
            {
                return;
            }

            var queryCommand = interceptionContext.Result as DbQueryCommandTree;
            if (queryCommand != null)
            {
                interceptionContext.Result = HandleQueryCommand(queryCommand);
            }
        }

        private static DbCommandTree HandleQueryCommand(DbQueryCommandTree queryCommand)
        {
            var newQuery = queryCommand.Query.Accept(new SoftDeleteQueryVisitor());

            return new DbQueryCommandTree(
                queryCommand.MetadataWorkspace,
                queryCommand.DataSpace,
                newQuery
            );
        }
    }
}
