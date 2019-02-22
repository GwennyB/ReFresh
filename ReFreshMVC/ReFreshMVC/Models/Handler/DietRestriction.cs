using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Models.Handler
{
    public class DietRestriction : AuthorizationHandler<DietRestriction>, IAuthorizationRequirement
    {

        /// <summary>
        /// establishes product access restrictions based on user's dietary preferences
        /// </summary>
        /// <param name="context"> authorization handler context </param>
        /// <param name="requirement"> dietary restriction requirement </param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DietRestriction requirement)
        {
            string carnivore = context.User.Claims.FirstOrDefault(c => c.Type == "Carnivore").Value;

            if (carnivore == "true")
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }

}
