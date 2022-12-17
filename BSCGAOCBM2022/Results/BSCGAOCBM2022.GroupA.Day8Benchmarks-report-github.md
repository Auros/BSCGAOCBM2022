``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2364/21H2/November2021Update)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|          Method |         Mean |     Error |          Ratio |     Gen0 |    Gen1 | Allocated | Alloc Ratio |
|---------------- |-------------:|----------:|---------------:|---------:|--------:|----------:|------------:|
|     Auros_Part1 |     89.04 μs |  0.460 μs |       baseline |   8.6670 |  1.4648 |   73152 B |             |
|    Caeden_Part1 | 12,159.41 μs | 30.379 μs | 136.57x slower | 484.3750 |       - | 4157430 B | 56.83x more |
| Eris_Part1_Sol1 |  2,842.52 μs |  9.461 μs |  31.90x slower | 644.5313 | 89.8438 | 5392666 B | 73.72x more |
| Eris_Part1_Sol2 |    393.08 μs |  1.788 μs |   4.41x slower |   3.9063 |       - |   35640 B |  2.05x less |
|                 |              |           |                |          |         |           |             |
|     Auros_Part2 |    482.20 μs |  1.313 μs |       baseline |        - |       - |         - |          NA |
|    Caeden_Part2 |  4,895.41 μs | 13.365 μs |  10.15x slower | 960.9375 |       - | 8059571 B |          NA |
| Eris_Part2_Sol1 |  2,633.59 μs |  8.542 μs |   5.46x slower | 644.5313 | 89.8438 | 5392746 B |          NA |
| Eris_Part2_Sol2 |    428.09 μs |  3.663 μs |   1.13x faster |   3.9063 |       - |   35640 B |          NA |
