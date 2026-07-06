# FrameIt MR

> **Mixed Reality interior design — visualize photo arrangements on your walls before picking up a hammer.**

---

## Demo

![FrameIt MR Demo](ezgif.com-optimize.gif)

---

## Overview

Hanging art is a guessing game. Will it fit? Do the colors clash? Is the spacing right? FrameIt MR eliminates that guesswork by letting you place, resize, and rearrange virtual picture frames directly on your real walls through mixed reality — no holes, no measuring tape, no regrets.

Built as a Meta Quest prototype, FrameIt MR demonstrates how MR can make everyday interior decisions faster, smarter, and more confident.

---

## Features

### 🖐 Hand Tracking — Grab & Resize
Reach out and grab any frame with your bare hands. Use both hands to scale it up or down. No controllers needed — the interaction feels as natural as handling a real picture frame.

### 📁 Android File Upload
Load any photo straight from your device storage. Powered by [UnitySimpleFileBrowser](https://github.com/yasirkula/UnitySimpleFileBrowser), the native file picker lets you browse and select images without leaving the MR experience.

### 📷 In-Headset Camera Capture
Can't find the right photo? Take one on the spot. A built-in capture flow uses the passthrough camera with a countdown timer and a satisfying camera flash effect to snap a photo and instantly frame it.

### 🧱 Scene Understanding (MRUK)
Frames are wall-aware. Using Meta's MR Utility Kit (MRUK), paintings can auto-snap to detected wall surfaces — ensuring they sit flush and straight every time.

### ✨ Polished Feedback
Particle effects on spawn and wall placement, spatial audio for the countdown, shutter sound, and wall snap — every interaction has a satisfying response.

---

## Tech Stack

| Component | Technology |
|---|---|
| Engine | Unity (Meta XR SDK) |
| Platform | Meta Quest (Android) |
| Hand Interaction | Meta Interaction SDK (`GrabFreeTransformer`) |
| Scene Understanding | Meta MR Utility Kit (MRUK) |
| File Picker | [UnitySimpleFileBrowser](https://github.com/yasirkula/UnitySimpleFileBrowser) |
| Camera | Passthrough Camera API (`WebCamTextureManager`) |
| UI | TextMesh Pro |

---

## Project Structure

```
Assets/
├── Scripts/
│   ├── ImageLoader.cs     # File picker + camera capture + frame spawning
│   └── Painting.cs        # Frame behavior: texture, scale, wall snap, FX
├── Scenes/
│   └── Main.unity         # Single main scene
├── Prefabs/               # Frame prefabs with interaction components
├── Audio/                 # Shutter, countdown, and wall placement sounds
└── PassthroughCameraApiSamples/  # Camera passthrough utilities
```

---

## How It Works

1. **Load** a photo from your device or **capture** one with the headset camera
2. A framed version of your photo spawns in front of you
3. **Grab** the frame with one or two hands to move and resize it
4. Point it at a wall — it **snaps** flush to the surface automatically
5. Step back and see how it looks in your actual room

---

## Getting Started

### Requirements
- Meta Quest 2 / 3 / Pro
- Unity 2022.3+ with Meta XR SDK
- Scene Understanding permission enabled in OVR Project Config
- `CAMERA` and `READ_EXTERNAL_STORAGE` Android permissions

### Setup
1. Clone the repo
2. Open the project in Unity
3. Open `Assets/Scenes/Main.unity`
4. Build and deploy to your Quest via Android build target

> Make sure Scene Understanding is enabled in your **OVR Manager** component and that passthrough is set up in your project settings.

---

## Dependencies

- [Meta XR SDK](https://developer.oculus.com/downloads/unity/)
- [Meta MR Utility Kit (MRUK)](https://developer.oculus.com/documentation/unity/unity-mr-utility-kit-overview/)
- [UnitySimpleFileBrowser](https://github.com/yasirkula/UnitySimpleFileBrowser) by yasirkula

---

## License

This project is a prototype built for exploratory/demonstration purposes.
