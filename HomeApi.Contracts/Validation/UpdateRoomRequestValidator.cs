using FluentValidation;
using HomeApi.Contracts.Models.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeApi.Contracts.Validation
{
    public class UpdateRoomRequestValidator : AbstractValidator<UpdateRoomRequest>
    {
        public UpdateRoomRequestValidator()
        {
            RuleFor(x => x.NewName).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Voltage).NotEmpty().InclusiveBetween(120,220);
            RuleFor(x =>x.Area).NotEmpty();
            RuleFor(x => x.GasConnected).NotNull();
        }
    }
}
