using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMS9900Translating;
using TMS9900Translating.Translating;

namespace TMS9900TranslatingTests
{
    [TestFixture]
    public class CallTests
    {
        [Test]
        public void Call_MemoryAddress()
        {
            var z80SourceCommand = "    call 48A1h";
            var z80Command = new Z80AssemblyParsing.Parsing.Z80LineParser().ParseLine(z80SourceCommand);
            var accumulator = new AfterthoughAccumulator();
            var translator = new TMS9900Translator(
                new List<(Z80SourceRegister, WorkspaceRegister)>(),
                new List<MemoryMapElement>(),
                accumulator
            );
            var tmsCommand = translator.Translate(z80Command).ToList();

            Assert.AreEqual(2, tmsCommand.Count);
            Assert.AreEqual("!can only translate a call command if it is to a labeled address", tmsCommand[0].CommandText);
            Assert.AreEqual("!    call 48A1h", tmsCommand[1].CommandText);
        }

        [Test]
        public void Call_LabeledAddress()
        {
            var z80SourceCommand = "    call otherRoutine";
            var z80Command = new Z80AssemblyParsing.Parsing.Z80LineParser().ParseLine(z80SourceCommand);
            var accumulator = new AfterthoughAccumulator();
            var translator = new TMS9900Translator(
                new List<(Z80SourceRegister, WorkspaceRegister)>(),
                new List<MemoryMapElement>(),
                accumulator
            );
            var tmsCommand = translator.Translate(z80Command).ToList();

            Assert.AreEqual(1, tmsCommand.Count);
            Assert.AreEqual("       BL   @otherRoutine", tmsCommand[0].CommandText);
            Assert.IsTrue(accumulator.LabelsBranchedTo.Contains("otherRoutine"));
        }
    }
}