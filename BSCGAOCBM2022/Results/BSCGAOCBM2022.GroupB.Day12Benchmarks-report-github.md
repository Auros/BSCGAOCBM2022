``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2364/21H2/November2021Update)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|       Method |        Mean |    Error |         Ratio |   Gen0 | Allocated |     Alloc Ratio |
|------------- |------------:|---------:|--------------:|-------:|----------:|----------------:|
|  Auros_Part1 |    33.42 μs | 0.566 μs |      baseline |      - |         - |              NA |
| Caeden_Part1 |    91.36 μs | 0.262 μs |  2.73x slower | 0.1221 |    1144 B |              NA |
|   Eris_Part1 |    20.63 μs | 0.075 μs |  1.62x faster |      - |         - |              NA |
|              |             |          |               |        |           |                 |
|  Auros_Part2 | 1,086.50 μs | 9.871 μs |      baseline |      - |       1 B |                 |
| Caeden_Part2 |   114.66 μs | 0.374 μs |  9.48x faster | 1.9531 |   16600 B | 16,600.00x more |
|   Eris_Part2 |    20.04 μs | 0.083 μs | 54.21x faster |      - |         - |              NA |
