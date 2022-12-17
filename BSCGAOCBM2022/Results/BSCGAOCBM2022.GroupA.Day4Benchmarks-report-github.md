``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2364/21H2/November2021Update)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|       Method |       Mean |    Error |         Ratio |     Gen0 |    Gen1 |  Allocated | Alloc Ratio |
|------------- |-----------:|---------:|--------------:|---------:|--------:|-----------:|------------:|
|  Auros_Part1 | 2,997.2 μs | 29.66 μs |      baseline | 132.8125 |       - | 1105.01 KB |             |
| Caeden_Part1 |   412.7 μs |  1.45 μs |  7.26x faster | 116.6992 | 22.4609 |  954.76 KB |  1.16x less |
|   Eris_Part1 |   533.8 μs |  1.56 μs |  5.62x faster |  91.7969 |       - |  753.18 KB |  1.47x less |
| Goobie_Part1 |   148.7 μs |  1.01 μs | 20.14x faster |  36.6211 |       - |   299.9 KB |  3.68x less |
|              |            |          |               |          |         |            |             |
|  Auros_Part2 |   576.0 μs |  6.04 μs |      baseline |  60.5469 |       - |  497.86 KB |             |
| Caeden_Part2 | 1,515.8 μs |  8.11 μs |  2.63x slower | 199.2188 | 39.0625 | 1629.96 KB |  3.27x more |
|   Eris_Part2 |   517.2 μs |  2.34 μs |  1.11x faster |  91.7969 |       - |  753.18 KB |  1.51x more |
| Goobie_Part2 |   147.1 μs |  0.79 μs |  3.92x faster |  36.6211 |       - |   299.9 KB |  1.66x less |
