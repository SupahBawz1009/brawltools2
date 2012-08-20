﻿using System;

namespace System
{
    public static class VQuant
    {
        public static float[] DeQuantTable = new float[] 
        { 
            1.0f / (1 << 0),         1.0f / (1 << 1),         1.0f / (1 << 2),         1.0f / (1 << 3),
            1.0f / (1 << 4),         1.0f / (1 << 5),         1.0f / (1 << 6),         1.0f / (1 << 7),
            1.0f / (1 << 8),         1.0f / (1 << 9),         1.0f / (1 << 10),        1.0f / (1 << 11),
            1.0f / (1 << 12),        1.0f / (1 << 13),        1.0f / (1 << 14),        1.0f / (1 << 15),
            1.0f / (1 << 16),        1.0f / (1 << 17),        1.0f / (1 << 18),        1.0f / (1 << 19),
            1.0f / (1 << 20),        1.0f / (1 << 21),        1.0f / (1 << 22),        1.0f / (1 << 23),
            1.0f / (1 << 24),        1.0f / (1 << 25),        1.0f / (1 << 26),        1.0f / (1 << 27),
            1.0f / (1 << 28),        1.0f / (1 << 29),        1.0f / (1 << 30),        1.0f / (1 << 31),
            (long)1 << 32,  1 << 31,      1 << 30,      1 << 29,
            1 << 28,      1 << 27,      1 << 26,      1 << 25,
            1 << 24,      1 << 23,      1 << 22,      1 << 21,
            1 << 20,      1 << 19,      1 << 18,      1 << 17,
            1 << 16,      1 << 15,      1 << 14,      1 << 13,
            1 << 12,      1 << 11,      1 << 10,      1 <<  9,
            1 <<  8,      1 <<  7,      1 <<  6,      1 <<  5,
            1 <<  4,      1 <<  3,      1 <<  2,      1 <<  1
        };

        public static readonly float[] QuantTable = new float[]
        {
        (1 <<  0),      (1 <<  1),      (1 <<  2),      (1 <<  3),
        (1 <<  4),      (1 <<  5),      (1 <<  6),      (1 <<  7),
        (1 <<  8),      (1 <<  9),      (1 << 10),      (1 << 11),
        (1 << 12),      (1 << 13),      (1 << 14),      (1 << 15),
        (1 << 16),      (1 << 17),      (1 << 18),      (1 << 19),
        (1 << 20),      (1 << 21),      (1 << 22),      (1 << 23),
        (1 << 24),      (1 << 25),      (1 << 26),      (1 << 27),
        (1 << 28),      (1 << 29),      (1 << 30),      (1 << 31),
        1.0f / ((long)1 << 32),     1.0f / (1 << 31),        1.0f / (1 << 30),        1.0f / (1 << 29),
        1.0f / (1 << 28),        1.0f / (1 << 27),        1.0f / (1 << 26),        1.0f / (1 << 25),
        1.0f / (1 << 24),        1.0f / (1 << 23),        1.0f / (1 << 22),        1.0f / (1 << 21),
        1.0f / (1 << 20),        1.0f / (1 << 19),        1.0f / (1 << 18),        1.0f / (1 << 17),
        1.0f / (1 << 16),        1.0f / (1 << 15),        1.0f / (1 << 14),        1.0f / (1 << 13),
        1.0f / (1 << 12),        1.0f / (1 << 11),        1.0f / (1 << 10),        1.0f / (1 <<  9),
        1.0f / (1 <<  8),        1.0f / (1 <<  7),        1.0f / (1 <<  6),        1.0f / (1 <<  5),
        1.0f / (1 <<  4),        1.0f / (1 <<  3),        1.0f / (1 <<  2),        1.0f / (1 <<  1),
        };
    }
}
