# ActivityRecognizer Plugin
A cross platform (Android only atm) way of getting the user activity using reactive structure.

## Android
Make sure you add the following to your `AndroidManifest.xml` file.
```xml
<uses-permission android:name="com.google.android.gms.permission.ACTIVITY_RECOGNITION" />
```

## iOS
Not available yet.
Make sure you add the following to your `Info.plist` file.
```xml
<key>NSMotionUsageDescription</key>
<string>Enter your usage description here.</string>
```

## UWP
Not supported.