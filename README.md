# StrongRandom

[![NuGet version (StrongRandom)](https://img.shields.io/nuget/v/StrongRandom.svg)](https://www.nuget.org/packages/StrongRandom/)
[![Licence (StrongRandom)](https://img.shields.io/github/license/mashape/apistatus.svg)](https://choosealicense.com/licenses/mit/)

```System.Random``` interface implemented with Cryptographically-Secure Pseudo-Random Number Generator. 
Everywhere you use System.Random, StrongRandom can be used. 

**Important!
Do not use with security critical scenarios, as the distribution will be somewhat skewed in some method calls, due to division.**

Extensions for any Random: NextBool, NextChar, NextString etc. added.
