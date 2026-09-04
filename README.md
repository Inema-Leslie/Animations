# Interactive Character Experience

## Project Description
The **Interactive Character Experience** is an interactive Unity application designed to showcase dynamic character behaviors, emotional expressions, and synchronized audiovisual feedback. Users can switch between distinct 3D character models in real time and trigger specific emotional states (Joy, Anger, Sadness). Each reaction drives state transitions within an Animator Controller while dynamically orchestrating sound effects and background audio ducking.

---

## Controls and Interactions

* **Character Selection:** Click the on-screen character buttons to switch the active 3D character model.
* **Emotion Triggers:**
  * **Happy Button:** Triggers the happy animation reaction and plays the corresponding cheer SFX.
  * **Anger Button:** Triggers the angry animation state and plays the anger SFX.
  * **Sadness Button:** Triggers the sad reaction and plays the crying SFX.
* **Audio Controls:**
  * **Master Volume Slider:** Dynamically adjusts global audio levels across background ambience and sound effects using a logarithmic curve.
  * **Mute Toggle:** Instantly mutes or restores all game audio without losing track positions.

---

## System Architecture

The project is structured around modular managers using the Singleton pattern to maintain a clear separation of concerns:

* **`GameManager`:** Controls core session logic and character swapping. It manages active character states using boundary validation (`Mathf.Clamp`) to prevent index out-of-range errors and notifies UI systems upon state changes.
* **`CharacterMoodController`:** Communicates directly with the character's `Animator` component. It fires triggers (`TriggerJoy`, `TriggerAnger`, `TriggerSadness`) and updates mood parameters, routing from the `Any State` node into distinct reaction clips before smoothly transitioning back to `Idle`.
* **`AudioManager`:** Manages continuous background ambience (`bgmSource`) and one-shot emotion sounds (`sfxSource`). It synchronizes sound playback with animation clip durations via coroutines, automatically pausing background music and applying a smooth volume fade-out (`Mathf.Lerp`) at the end of each reaction.
* **`UIManager`:** Handles HUD updates, reflecting current character names, active emotional states, and routing UI button interactions directly to the appropriate manager scripts.
