﻿using System;

namespace TMS9900Translating
{
    public enum WorkspaceRegister
    {
        R1, R2, R3, R4, R5, R6, R7, R8, R9
    }

    public struct RegisterMapElement
    {
        public Z80AssemblyParsing.Register Z80Register { get; set; }
        public WorkspaceRegister TMS900Register { get; set; }
    }

    public struct MemoryMapElement
    {
        public ushort Z80Start { get; set; }
        public ushort Z80End => (ushort)(Z80Start + BlockLength - 1);
        public ushort TMS9900Start { get; set; }
        public ushort TMS9000End => (ushort)(TMS9900Start + BlockLength - 1);
        public ushort BlockLength { get; set; }
    }
}