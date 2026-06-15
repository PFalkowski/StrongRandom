# StrongRandom

[![CI](https://github.com/PFalkowski/StrongRandom/actions/workflows/ci.yml/badge.svg)](https://github.com/PFalkowski/StrongRandom/actions/workflows/ci.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=PFalkowski_StrongRandom&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=PFalkowski_StrongRandom)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=PFalkowski_StrongRandom&metric=coverage)](https://sonarcloud.io/summary/new_code?id=PFalkowski_StrongRandom)
[![NuGet version](https://img.shields.io/nuget/v/StrongRandom.svg)](https://www.nuget.org/packages/StrongRandom/)
[![NuGet downloads](https://img.shields.io/nuget/dt/StrongRandom.svg)](https://www.nuget.org/packages/StrongRandom/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Buy Me A Coffee](https://img.shields.io/badge/Buy%20Me%20A%20Coffee-support-yellow.svg)](https://www.buymeacoffee.com/piotrfalkowski)

`System.Random` drop-in backed by a **Cryptographically-Secure Pseudo-Random Number Generator (CSPRNG)**. Everywhere you use `System.Random`, `StrongRandom` can be used instead. Also ships extension methods that add `NextBool`, `NextByte`, `NextChar`, `NextFloat`, `NextNormal` and more to _any_ `Random` instance.

> **Security note:** `NextBytes()` is a direct call to the underlying CSPRNG and is safe. Integer-returning methods (`Next`, `Next(max)`, `Next(min, max)`) and `NextDouble()` involve a transformation step that introduces a slight bias — do **not** rely on these for security-critical decisions such as key generation. Use `NextBytes()` for raw entropy.

## Installation

```
dotnet add package StrongRandom
```

## Quick start

```csharp
using Extensions.Standard.Randomization;

// Drop-in: assign to a System.Random variable
Random rng = new StrongRandom();

int dice   = rng.Next(1, 7);          // 1–6
double d   = rng.NextDouble();        // [0, 1)
byte[] key = new byte[32];
rng.NextBytes(key);                   // CSPRNG-safe raw bytes
```

## Extension methods (`Utilities`)

All extensions target `System.Random`, so they work with `StrongRandom`, plain `new Random()`, or a mock.

| Method | Signature | Description |
|--------|-----------|-------------|
| `NextByte` | `(short upperLimit = 256)` | Random byte in `[0, upperLimit)` |
| `NextBool` | `()` | `true` or `false` with equal probability |
| `NextChar` | `(char lower = ' ', char upper = '\x7F')` | Random character in printable ASCII range |
| `NextChar` | `(string chooseFrom)` | Random character from a specific set |
| `NextLowercaseLetter` | `()` | Random letter `a`–`z` |
| `NextUppercaseLetter` | `()` | Random letter `A`–`Z` |
| `NextLetter` | `()` | Random letter `a`–`z` or `A`–`Z` |
| `NextAlphanumeric` | `()` | Random digit, uppercase, or lowercase letter |
| `NextFloat` | `()` | Random `float` in `[0, 1)` |
| `NextFloat` | `(float min, float max)` | Random `float` in `[min, max)` |
| `NextDouble` | `(double max)` | Random `double` in `[0, max)` |
| `NextDouble` | `(double min, double max)` | Random `double` in `[min, max)` |
| `NextNormal` | `(double mean, double sd = 1)` | Normally distributed value (Box-Muller) |

```csharp
var rng = new StrongRandom();

bool coinFlip       = rng.NextBool();
byte b              = rng.NextByte();
char letter         = rng.NextLetter();
char alphanumeric   = rng.NextAlphanumeric();
float f             = rng.NextFloat(0f, 1f);
double normal       = rng.NextNormal(mean: 0, sd: 1);
char fromSet        = rng.NextChar("AEIOU");
```

## Customising the provider

`StrongRandom` depends on `IRandomProvider` for its byte source. The default is `BufferedRandomProvider(44)`, which pre-fetches 44 bytes per CSPRNG call to reduce overhead.

```csharp
// Larger buffer — fewer round-trips to the CSPRNG
var rng = new StrongRandom(new BufferedRandomProvider(256));

// Custom provider (e.g. for testing)
public class MyProvider : IRandomProvider
{
    public void GetBytes(byte[] input) { /* ... */ }
}
var testRng = new StrongRandom(new MyProvider());
```

## API

### `StrongRandom`

Inherits from `System.Random`. All `Random` members work as expected; randomness comes from the CSPRNG provider.

| Member | Notes |
|--------|-------|
| `StrongRandom(IRandomProvider? provider = null)` | `null` uses `BufferedRandomProvider(44)` |
| `Next()` | CSPRNG-backed |
| `Next(int maxValue)` | CSPRNG-backed |
| `Next(int minValue, int maxValue)` | CSPRNG-backed |
| `NextDouble()` | CSPRNG-backed |
| `NextBytes(byte[] buffer)` | Direct CSPRNG fill — unbiased |

### `BufferedRandomProvider`

| Member | Notes |
|--------|-------|
| `BufferedRandomProvider(int bufferSize)` | Pre-fetches `bufferSize` bytes per CSPRNG call |
| `GetBytes(byte[] input)` | Fills `input` from the buffer, refreshing as needed |

### `IRandomProvider`

```csharp
public interface IRandomProvider
{
    void GetBytes(byte[] input);
}
```

## License

MIT — see [LICENSE](LICENSE).
