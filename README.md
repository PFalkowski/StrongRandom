# StrongRandom

[![NuGet version (StrongRandom)](https://img.shields.io/nuget/v/StrongRandom.svg)](https://www.nuget.org/packages/StrongRandom/)
[![Licence (StrongRandom)](https://img.shields.io/github/license/mashape/apistatus.svg)](https://choosealicense.com/licenses/mit/)

```System.Random``` interface implemented with Cryptographically-Secure Pseudo-Random Number Generator. 
Everywhere you use System.Random, StrongRandom can be used. 

**Important!
Use with caution in security critical scenarios, as the distribution can be somewhat skewed in integer returning method calls and NextDouble(). GetBytes() is direct call to underlying RNG provider and is safe.**

Extensions for any Random: NextBool, NextChar, NextString etc. added.
