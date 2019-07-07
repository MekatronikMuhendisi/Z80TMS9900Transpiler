﻿using System.Collections.Generic;
using TMS9900Translating.Commands;
using TmsCommand = TMS9900Translating.Command;

namespace TMS9900Translating.Translating
{
    public class CallCommandTranslator : CommandTranslator<Z80AssemblyParsing.Commands.UnconditionalCallCommand>
    {
        public CallCommandTranslator(MapCollection mapCollection) : base(mapCollection)
        {
        }

        public override IEnumerable<TmsCommand> Translate(Z80AssemblyParsing.Commands.UnconditionalCallCommand callCommand)
        {
            if (callCommand.Operand is Z80AssemblyParsing.Operands.LabeledAddressWithoutParenthesisOperand)
            {
                var destinationOperand = GetOperand(callCommand.Operand, false);
                yield return new BranchLinkCommand(callCommand, destinationOperand);
            }
            else
            {
                yield return new UntranslateableComment(callCommand, "can only translate a call command if it is to a labeled address");
                yield return new UntranslateableComment(callCommand, callCommand.SourceText);
            }
        }
    }
}