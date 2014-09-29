Betwixt - Easy Tweening Library for .Net
============================

Betwixt is a quick way to ease and tween between any generic types, and provides a robust fast API to allow you to do
whatever you set out to do, in the easiest way possible.

# Download

You can get the latest repository here, or download precompiled library files from the [releases tab](https://github.com/Jewelots/Betwixt/releases)

You can also get this package using Nuget!

For x86: PM > Install-Package betwixt   
For x64: PM > Install-Package betwixt_x64

# Usage

Please reference the documentation located in the [/doc](/doc) folder. Here is a very small overview, however:

General Use:
```csharp
// Initialisation
Tweener<float> tweener = new Tweener<float>(0, 10, 2, Ease.Elastic.Out);
// Update
tweener.Update(deltaTime);
// Anywhere
float newValue = tweener.Value;
```


You can also use your own custom type, with it's own lerp function (or let generics handle it)

```csharp
TimeSpan length = TimeSpan.FromSeconds(3);
Tweener<Vector2> tweener = new Tweener<Vector2>(startVector, endVector, length, Ease.Linear, Vector2.Lerp);
```


You can also specify your own ease function and make it into a set (or use the function directly)

```csharp
IEase myEaseSet = Generic.CreateFromOut(myEaseOutFunction);
Tweener<float> tweener = new Tweener<float>(0, 10, 2, myEaseSet.InOut);
```


Betwixt is incredibly flexible, and as long as your ease function matches the correct signature, you can use anything you want!

If you had a custom graph curve which had a "float GetValueAtTime(float time)" function you could even use:

```csharp
CustomGraphCurve myCustomGraphCurve = new CustomGraphCurve(graphPoints);

Tweener<float> tweener = new Tweener<float>(0, 10, 2, myCustomGraphCurve.GetValueAtTime);
```


Hopefully this inspires you to think of creative ways to use Betwixt!

# Contact

Feel free to contact me at [twitchy137@gmail.com](mailto:twitchy137@gmail.com?subject=FFXIV Server Status Notifier) if you have any questions, or maybe if you want to toss me a few cents!
