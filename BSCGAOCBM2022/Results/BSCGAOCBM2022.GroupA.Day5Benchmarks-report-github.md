``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2364/21H2/November2021Update)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|      Method |      Mean |    Error |        Ratio |    Gen0 |   Gen1 | Allocated | Alloc Ratio |
|------------ |----------:|---------:|-------------:|--------:|-------:|----------:|------------:|
| Auros_Part1 | 443.39 μs | 1.605 μs |     baseline | 15.6250 |      - | 128.44 KB |             |
|  Eris_Part1 |  81.91 μs | 0.351 μs | 5.41x faster | 16.8457 | 0.3662 | 138.03 KB |  1.07x more |
|             |           |          |              |         |        |           |             |
| Auros_Part2 | 522.62 μs | 1.591 μs |     baseline | 25.3906 |      - | 209.89 KB |             |
|  Eris_Part2 |  91.47 μs | 0.668 μs | 5.71x faster | 21.1182 | 0.4883 | 172.57 KB |  1.22x less |
