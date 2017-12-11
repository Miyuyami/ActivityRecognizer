# ActivityRecognizer Plugin
A cross platform (Android and iOS only atm) way of getting the user activity using reactive structure.

## Android
Make sure you add the following to your `AndroidManifest.xml` file.
```xml
<uses-permission android:name="com.google.android.gms.permission.ACTIVITY_RECOGNITION" />
```

## iOS
Make sure you add the following to your `Info.plist` file.
```xml
<key>NSMotionUsageDescription</key>
<string>Enter your usage description here.</string>
```
### iOS limitations
* usually it only sends one type of activity.
* the confidence is an enum instead of precentages like on android, Low, Medium, High and they are translated to 15, 50 and respectively 90.

## UWP
Not supported.

# Examples and how to use
The monitoring starts at object creation so a good idea is to call the current instance at the start of the app so that the lazy implementation is created.
```csharp
void OnStart()
{
	if (Plugin.ActivityRecognizer.CrossActivityRecognizer.Current.IsSupported)
	{
		// good to go
	}
}
```

Log all activities:
```csharp
Plugin.ActivityRecognizer.CrossActivityRecognizer.Current.WhenMotionDetected().Subscribe(r => 
{
	MyLogger.Write($"Detected a motion at {r.Timestamp}.");

	MyLogger.Write($"Most probable motion: {r.MostProbableMotion}.");

	MyLogger.Write($"Detected motions:");
	foreach (var motion in r.DetectedMotions)
	{
		MyLogger.Write($"{motion.Confidence}% confident for {motion.Type}.");
	}
});
```

Get last activity, be careful that if there is no last detected motion, it will await until one is detected, this is why you want to initialize the instance at the start of the app.
```csharp
using System.Reactive.Linq;
var motionResult = await Plugin.ActivityRecognizer.CrossActivityRecognizer.Current.GetMotion();

MyLogger.Write($"Last most probable motion: {motionResult.MostProbableMotion}");
```