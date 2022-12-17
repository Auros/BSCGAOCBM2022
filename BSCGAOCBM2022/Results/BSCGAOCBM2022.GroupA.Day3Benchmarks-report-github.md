``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2364/21H2/November2021Update)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|       Method |      Mean |    Error |         Ratio |    Gen0 |   Gen1 | Allocated | Alloc Ratio |
|------------- |----------:|---------:|--------------:|--------:|-------:|----------:|------------:|
|  Auros_Part1 | 267.97 μs | 0.897 μs |      baseline | 42.9688 |      - |  359528 B |             |
| Caeden_Part1 | 232.55 μs | 1.196 μs |  1.15x faster | 36.1328 | 3.9063 |  303712 B |  1.18x less |
|   Eris_Part1 | 204.20 μs | 0.537 μs |  1.31x faster | 32.4707 |      - |  271592 B |  1.32x less |
| Goobie_Part1 |  10.78 μs | 0.187 μs | 24.86x faster |       - |      - |         - |          NA |
|              |           |          |               |         |        |           |             |
|  Auros_Part2 | 191.71 μs | 0.506 μs |      baseline | 34.9121 | 0.2441 |  292704 B |             |
| Caeden_Part2 | 379.29 μs | 0.932 μs |  1.98x slower | 34.1797 | 4.8828 |  286064 B |  1.02x less |
|   Eris_Part2 | 213.86 μs | 0.496 μs |  1.12x slower | 27.8320 |      - |  233248 B |  1.25x less |
| Goobie_Part2 |  20.48 μs | 0.405 μs |  9.36x faster |       - |      - |         - |          NA |
