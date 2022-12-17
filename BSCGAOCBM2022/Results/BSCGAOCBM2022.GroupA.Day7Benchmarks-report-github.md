``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2364/21H2/November2021Update)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|       Method |      Mean |    Error |        Ratio |    Gen0 |   Gen1 | Allocated | Alloc Ratio |
|------------- |----------:|---------:|-------------:|--------:|-------:|----------:|------------:|
|  Auros_Part1 |  80.40 μs | 0.341 μs |     baseline | 19.4092 | 3.4180 | 159.05 KB |             |
| Caeden_Part1 | 116.41 μs | 0.351 μs | 1.45x slower | 22.4609 | 4.3945 | 183.68 KB |  1.15x more |
|   Eris_Part1 | 173.27 μs | 0.844 μs | 2.16x slower | 29.5410 | 4.8828 | 241.35 KB |  1.52x more |
|              |           |          |              |         |        |           |             |
|  Auros_Part2 |  80.49 μs | 0.275 μs |     baseline | 19.4092 | 3.5400 | 159.26 KB |             |
| Caeden_Part2 | 130.41 μs | 0.768 μs | 1.62x slower | 24.1699 | 4.6387 | 198.15 KB |  1.24x more |
|   Eris_Part2 | 203.51 μs | 0.436 μs | 2.53x slower | 32.4707 | 5.3711 |  266.8 KB |  1.68x more |
