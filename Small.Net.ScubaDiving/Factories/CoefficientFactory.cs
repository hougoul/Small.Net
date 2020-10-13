using System;
using System.Collections.Generic;
using Small.Net.ScubaDiving.Entities;

namespace Small.Net.ScubaDiving.Factories
{
    public class CoefficientFactory : IDataFactory<Compartiment, BuhlmannCompartimentType>
    {
        public IList<Compartiment> CreateData(BuhlmannCompartimentType type)
        {
            List<Compartiment> compartiments = null;
            switch (type)
            {
                case BuhlmannCompartimentType.Helium:
                    compartiments = new List<Compartiment>()
                    {
                        new Compartiment()
                        {
                            Id = 1, PeriodMin = 1.88, CoefficientA = 1.6189, CoefficientB = 0.4770,
                            CoefficientK = Math.Log(2.0) / 1.88, Type = CompartimentType.Helium
                        },
                        new Compartiment()
                        {
                            Id = 2, PeriodMin = 3.02, CoefficientA = 1.3830, CoefficientB = 0.5747,
                            CoefficientK = Math.Log(2.0) / 3.02, Type = CompartimentType.Helium
                        },
                        new Compartiment()
                        {
                            Id = 3, PeriodMin = 4.72, CoefficientA = 1.1919, CoefficientB = 0.6527,
                            CoefficientK = Math.Log(2.0) / 4.72, Type = CompartimentType.Helium
                        },
                        new Compartiment()
                        {
                            Id = 4, PeriodMin = 6.99, CoefficientA = 1.0458, CoefficientB = 0.7223,
                            CoefficientK = Math.Log(2.0) / 6.99, Type = CompartimentType.Helium
                        },
                        new Compartiment()
                        {
                            Id = 5, PeriodMin = 10.21, CoefficientA = 0.9220, CoefficientB = 0.7582,
                            CoefficientK = Math.Log(2.0) / 10.21, Type = CompartimentType.Helium
                        },
                        new Compartiment()
                        {
                            Id = 6, PeriodMin = 14.48, CoefficientA = 0.8205, CoefficientB = 0.7957,
                            CoefficientK = Math.Log(2.0) / 14.48, Type = CompartimentType.Helium
                        },
                        new Compartiment()
                        {
                            Id = 7, PeriodMin = 20.53, CoefficientA = 0.7305, CoefficientB = 0.8279,
                            CoefficientK = Math.Log(2.0) / 20.53, Type = CompartimentType.Helium
                        },
                        new Compartiment()
                        {
                            Id = 8, PeriodMin = 29.11, CoefficientA = 0.6502, CoefficientB = 0.8553,
                            CoefficientK = Math.Log(2.0) / 29.11, Type = CompartimentType.Helium
                        },
                        new Compartiment()
                        {
                            Id = 9, PeriodMin = 41.20, CoefficientA = 0.5950, CoefficientB = 0.8757,
                            CoefficientK = Math.Log(2.0) / 41.20, Type = CompartimentType.Helium
                        },
                        new Compartiment()
                        {
                            Id = 10, PeriodMin = 55.19, CoefficientA = 0.5545, CoefficientB = 0.8903,
                            CoefficientK = Math.Log(2.0) / 55.19, Type = CompartimentType.Helium
                        },
                        new Compartiment()
                        {
                            Id = 11, PeriodMin = 70.69, CoefficientA = 0.5333, CoefficientB = 0.8997,
                            CoefficientK = Math.Log(2.0) / 70.69, Type = CompartimentType.Helium
                        },
                        new Compartiment()
                        {
                            Id = 12, PeriodMin = 90.34, CoefficientA = 0.5189, CoefficientB = 0.9073,
                            CoefficientK = Math.Log(2.0) / 90.34, Type = CompartimentType.Helium
                        },
                        new Compartiment()
                        {
                            Id = 13, PeriodMin = 115.29, CoefficientA = 0.5181, CoefficientB = 0.9122,
                            CoefficientK = Math.Log(2.0) / 115.29, Type = CompartimentType.Helium
                        },
                        new Compartiment()
                        {
                            Id = 14, PeriodMin = 147.42, CoefficientA = 0.5176, CoefficientB = 0.9171,
                            CoefficientK = Math.Log(2.0) / 147.42, Type = CompartimentType.Helium
                        },
                        new Compartiment()
                        {
                            Id = 15, PeriodMin = 188.24, CoefficientA = 0.5172, CoefficientB = 0.9217,
                            CoefficientK = Math.Log(2.0) / 188.24, Type = CompartimentType.Helium
                        },
                        new Compartiment()
                        {
                            Id = 16, PeriodMin = 240.03, CoefficientA = 0.5119, CoefficientB = 0.9267,
                            CoefficientK = Math.Log(2.0) / 240.03, Type = CompartimentType.Helium
                        }
                    };
                    break;
                case BuhlmannCompartimentType.AzoteA:
                    compartiments = new List<Compartiment>()
                    {
                        new Compartiment()
                        {
                            Id = 1, PeriodMin = 5.0, CoefficientA = 1.1696, CoefficientB = 0.5578,
                            CoefficientK = Math.Log(2.0) / 5.0, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 2, PeriodMin = 8.0, CoefficientA = 1.0, CoefficientB = 0.6514,
                            CoefficientK = Math.Log(2.0) / 8.0, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 3, PeriodMin = 12.5, CoefficientA = 0.8618, CoefficientB = 0.7222,
                            CoefficientK = Math.Log(2.0) / 12.5, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 4, PeriodMin = 18.5, CoefficientA = 0.7562, CoefficientB = 0.7825,
                            CoefficientK = Math.Log(2.0) / 18.5, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 5, PeriodMin = 27.0, CoefficientA = 0.6667, CoefficientB = 0.8126,
                            CoefficientK = Math.Log(2.0) / 27.0, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 6, PeriodMin = 38.3, CoefficientA = 0.5933, CoefficientB = 0.8434,
                            CoefficientK = Math.Log(2.0) / 38.3, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 7, PeriodMin = 54.3, CoefficientA = 0.5282, CoefficientB = 0.8693,
                            CoefficientK = Math.Log(2.0) / 54.3, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 8, PeriodMin = 77.0, CoefficientA = 0.4710, CoefficientB = 0.8910,
                            CoefficientK = Math.Log(2.0) / 77, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 9, PeriodMin = 109, CoefficientA = 0.4187, CoefficientB = 0.9092,
                            CoefficientK = Math.Log(2.0) / 109, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 10, PeriodMin = 146, CoefficientA = 0.3798, CoefficientB = 0.9222,
                            CoefficientK = Math.Log(2.0) / 146, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 11, PeriodMin = 187, CoefficientA = 0.3497, CoefficientB = 0.9319,
                            CoefficientK = Math.Log(2.0) / 187, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 12, PeriodMin = 239, CoefficientA = 0.3223, CoefficientB = 0.9403,
                            CoefficientK = Math.Log(2.0) / 239, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 13, PeriodMin = 305, CoefficientA = 0.2971, CoefficientB = 0.9477,
                            CoefficientK = Math.Log(2.0) / 305, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 14, PeriodMin = 390, CoefficientA = 0.2737, CoefficientB = 0.9544,
                            CoefficientK = Math.Log(2.0) / 390, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 15, PeriodMin = 498, CoefficientA = 0.2523, CoefficientB = 0.9602,
                            CoefficientK = Math.Log(2.0) / 498, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 16, PeriodMin = 635, CoefficientA = 0.2327, CoefficientB = 0.9653,
                            CoefficientK = Math.Log(2.0) / 635, Type = CompartimentType.Azote
                        }
                    };
                    break;
                case BuhlmannCompartimentType.AzoteB:
                    compartiments = new List<Compartiment>()
                    {
                        new Compartiment()
                        {
                            Id = 1, PeriodMin = 5.0, CoefficientA = 1.1696, CoefficientB = 0.5578,
                            CoefficientK = Math.Log(2.0) / 5.0, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 2, PeriodMin = 8.0, CoefficientA = 1.0, CoefficientB = 0.6514,
                            CoefficientK = Math.Log(2.0) / 8.0, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 3, PeriodMin = 12.5, CoefficientA = 0.8618, CoefficientB = 0.7222,
                            CoefficientK = Math.Log(2.0) / 12.5, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 4, PeriodMin = 18.5, CoefficientA = 0.7562, CoefficientB = 0.7825,
                            CoefficientK = Math.Log(2.0) / 18.5, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 5, PeriodMin = 27.0, CoefficientA = 0.6667, CoefficientB = 0.8126,
                            CoefficientK = Math.Log(2.0) / 27.0, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 6, PeriodMin = 38.3, CoefficientA = 0.5766, CoefficientB = 0.8434,
                            CoefficientK = Math.Log(2.0) / 38.3, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 7, PeriodMin = 54.3, CoefficientA = 0.5114, CoefficientB = 0.8693,
                            CoefficientK = Math.Log(2.0) / 54.3, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 8, PeriodMin = 77.0, CoefficientA = 0.4605, CoefficientB = 0.8910,
                            CoefficientK = Math.Log(2.0) / 77, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 9, PeriodMin = 109, CoefficientA = 0.4187, CoefficientB = 0.9092,
                            CoefficientK = Math.Log(2.0) / 109, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 10, PeriodMin = 146, CoefficientA = 0.3798, CoefficientB = 0.9222,
                            CoefficientK = Math.Log(2.0) / 146, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 11, PeriodMin = 187, CoefficientA = 0.3497, CoefficientB = 0.9319,
                            CoefficientK = Math.Log(2.0) / 187, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 12, PeriodMin = 239, CoefficientA = 0.3223, CoefficientB = 0.9403,
                            CoefficientK = Math.Log(2.0) / 239, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 13, PeriodMin = 305, CoefficientA = 0.2910, CoefficientB = 0.9477,
                            CoefficientK = Math.Log(2.0) / 305, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 14, PeriodMin = 390, CoefficientA = 0.2737, CoefficientB = 0.9544,
                            CoefficientK = Math.Log(2.0) / 390, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 15, PeriodMin = 498, CoefficientA = 0.2523, CoefficientB = 0.9602,
                            CoefficientK = Math.Log(2.0) / 498, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 16, PeriodMin = 635, CoefficientA = 0.2327, CoefficientB = 0.9653,
                            CoefficientK = Math.Log(2.0) / 635, Type = CompartimentType.Azote
                        }
                    };
                    break;
                case BuhlmannCompartimentType.AzoteC:
                    compartiments = new List<Compartiment>()
                    {
                        new Compartiment()
                        {
                            Id = 1, PeriodMin = 5.0, CoefficientA = 1.1696, CoefficientB = 0.5578,
                            CoefficientK = Math.Log(2.0) / 5.0, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 2, PeriodMin = 8.0, CoefficientA = 1.0, CoefficientB = 0.6514,
                            CoefficientK = Math.Log(2.0) / 8.0, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 3, PeriodMin = 12.5, CoefficientA = 0.8618, CoefficientB = 0.7222,
                            CoefficientK = Math.Log(2.0) / 12.5, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 4, PeriodMin = 18.5, CoefficientA = 0.7562, CoefficientB = 0.7825,
                            CoefficientK = Math.Log(2.0) / 18.5, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 5, PeriodMin = 27.0, CoefficientA = 0.6667, CoefficientB = 0.8126,
                            CoefficientK = Math.Log(2.0) / 27.0, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 6, PeriodMin = 38.3, CoefficientA = 0.56, CoefficientB = 0.8434,
                            CoefficientK = Math.Log(2.0) / 38.3, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 7, PeriodMin = 54.3, CoefficientA = 0.4947, CoefficientB = 0.8693,
                            CoefficientK = Math.Log(2.0) / 54.3, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 8, PeriodMin = 77.0, CoefficientA = 0.45, CoefficientB = 0.8910,
                            CoefficientK = Math.Log(2.0) / 77, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 9, PeriodMin = 109, CoefficientA = 0.4187, CoefficientB = 0.9092,
                            CoefficientK = Math.Log(2.0) / 109, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 10, PeriodMin = 146, CoefficientA = 0.3798, CoefficientB = 0.9222,
                            CoefficientK = Math.Log(2.0) / 146, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 11, PeriodMin = 187, CoefficientA = 0.3497, CoefficientB = 0.9319,
                            CoefficientK = Math.Log(2.0) / 187, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 12, PeriodMin = 239, CoefficientA = 0.3223, CoefficientB = 0.9403,
                            CoefficientK = Math.Log(2.0) / 239, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 13, PeriodMin = 305, CoefficientA = 0.2850, CoefficientB = 0.9477,
                            CoefficientK = Math.Log(2.0) / 305, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 14, PeriodMin = 390, CoefficientA = 0.2737, CoefficientB = 0.9544,
                            CoefficientK = Math.Log(2.0) / 390, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 15, PeriodMin = 498, CoefficientA = 0.2523, CoefficientB = 0.9602,
                            CoefficientK = Math.Log(2.0) / 498, Type = CompartimentType.Azote
                        },
                        new Compartiment()
                        {
                            Id = 16, PeriodMin = 635, CoefficientA = 0.2327, CoefficientB = 0.9653,
                            CoefficientK = Math.Log(2.0) / 635, Type = CompartimentType.Azote
                        }
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return compartiments;
        }
    }
}