``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2364/21H2/November2021Update)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|     Method |       Mean |    Error |   StdDev |
|----------- |-----------:|---------:|---------:|
| Eris_Part1 |   638.1 μs |  2.41 μs |  2.25 μs |
| Eris_Part2 | 2,363.2 μs | 17.15 μs | 16.04 μs |
