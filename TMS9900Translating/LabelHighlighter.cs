﻿using System;
using System.Collections.Generic;

namespace TMS9900Translating
{
    public class LabelHighlighter
    {
        private HashSet<string> _labelsBranchedTo = new HashSet<string>();
        public HashSet<string> LabelsFromZ80Code { get; } = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
        public IReadOnlyCollection<string> LabelsBranchedTo => _labelsBranchedTo;
        private int _nextInc = 0;
        private int _nextDec = 0;
        private int _nextJump = 0;
        private int _nextRt = 0;
        private string _negOneByteLabel;
        private string _oneByteLabel;
        private string _zeroByteLabel;

        public string NegOneByteLabel
        {
            get
            {
                return (_negOneByteLabel = (_negOneByteLabel ?? GetLabelUnusedByZ80("NEGONE")));
            }
        }

        public string ZeroByteLabel
        {
            get
            {
                return (_zeroByteLabel = (_zeroByteLabel ?? GetLabelUnusedByZ80("ZERO")));
            }
        }

        public string OneByteLabel
        {
            get
            {
                return (_oneByteLabel = (_oneByteLabel ?? GetLabelUnusedByZ80("ONE")));
            }
        }

        public void AddLabelToBranchTo(string label)
        {
            _labelsBranchedTo.Add(label);
        }

        public void ResetForEnumeration()
        {
            _nextInc = 0;
            _nextDec = 0;
        }

        public string GetNextIncLabel()
        {
            var incLabel = string.Empty;
            while (LabelsFromZ80Code.Contains(incLabel) || incLabel == string.Empty)
            {
                var nextInc = ++_nextInc;
                incLabel = "INC" + new string('0', 2 - (int)Math.Log10(nextInc)) + nextInc.ToString();
            }
            return incLabel;
        }

        public string GetNextDecLabel()
        {
            var decLabel = string.Empty;
            while (LabelsFromZ80Code.Contains(decLabel) || decLabel == string.Empty)
            {
                var nextDec = ++_nextDec;
                decLabel = "DEC" + new string('0', 2 - (int)Math.Log10(nextDec)) + nextDec.ToString();
            }
            return decLabel;
        }

        public string GetNextJumpLabel()
        {
            var jmpLabel = string.Empty;
            while (LabelsFromZ80Code.Contains(jmpLabel) || jmpLabel == string.Empty)
            {
                var nextJump = ++_nextJump;
                jmpLabel = "JMP" + new string('0', 2 - (int)Math.Log10(nextJump)) + nextJump.ToString();
            }
            return jmpLabel;
        }

        public string GetNextRtLabel()
        {
            var rtLabel = string.Empty;
            while (LabelsFromZ80Code.Contains(rtLabel) || rtLabel == string.Empty)
            {
                var nextRt = ++_nextRt;
                rtLabel = "RT" + new string('0', 3 - (int)Math.Log10(nextRt)) + nextRt.ToString();
            }
            return rtLabel;
        }

        public string GetLabelUnusedByZ80(string sourceLabel)
        {
            if (!LabelsFromZ80Code.Contains(sourceLabel))
                return sourceLabel;
            sourceLabel = sourceLabel + new string('0', 6 - sourceLabel.Length);
            while (LabelsFromZ80Code.Contains(sourceLabel))
                StringIncrementer.Increment(sourceLabel);
            return sourceLabel;
        }
    }
}
