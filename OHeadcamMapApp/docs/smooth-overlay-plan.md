# Smooth Overlay Plan

## Goal
Move from legacy per-second image-sequence overlays to smooth map overlays, while keeping the old path available until the new one is verified.

## Principles
- Keep legacy code path working during migration.
- Build and test preview and final render separately.
- Reuse the same smooth time/heading model in preview and export.
- Prefer small testable steps over large rewrites.

## Current Status
- Smooth rendering helpers exist in `clsMapImages.vb`.
- Smooth time-based video generation exists in `clsMapImages.vb`.
- Smooth heading and tail smoothing are being tuned.
- Legacy image-sequence output via `temp3\\%08d.png` is still the active final-render path.

## Work Phases

### 1. Smooth Preview In Adjustment Window
Files:
- `frmAdjustmentPlayerNew.vb`
- `clsMapImages.vb`

Tasks:
- Add a clean smooth preview path for `PB_Zoom` and `PBLeg`.
- Use `Double`-time rendering in the adjustment player.
- Keep legacy preview available if needed for comparison.

Tests:
- Pause on known times and verify zoom/leg match expected route position.
- Check that arrow, tail, colors, size, and map cropping reflect user settings.
- Check that scaling from panel space to final video space is still correct.

### 2. Smooth Overlay Asset Generation
Files:
- `clsMapImages.vb`
- possibly `MainForm.vb`

Tasks:
- Generate standalone smooth overlay videos for:
- zoom map
- leg map
- Ensure frame step and fps are derived consistently.
- Ensure output duration and timing match preview logic.

Tests:
- Verify output video length matches requested duration.
- Verify first/middle/last frames against preview.
- Verify both videos stay in sync with each other.

### 3. Final FFmpeg Overlay Path
Files:
- `MainForm.vb`
- `clsExtra.vb`

Tasks:
- Add a new final-render path using smooth overlay videos instead of `temp3` image sequences.
- Overlay the new zoom/leg videos at the stored positions and sizes.
- Keep old final-render path as fallback during migration.

Tests:
- Verify overlay positions match adjustment window layout.
- Verify GPS/video time offset is respected.
- Verify output duration, fps, and sync with base video.

### 4. Controlled Switching
Files:
- `MainForm.vb`
- settings/UI as needed

Tasks:
- Add a simple switch between legacy and smooth overlay output.
- Make it easy to compare old and new output on short clips.

Tests:
- Confirm both paths still run.
- Confirm switching does not alter stored layout/settings unexpectedly.

### 5. Cleanup And Cutover
Tasks:
- Decide when smooth path is reliable enough to become default.
- Reduce duplicated code where safe.
- Keep fallback path until confidence is high.

## Notes
- The adjustment window is the visual truth source. Final render should match it.
- Time step in smooth mode should be expressed in seconds, not primarily as fps.
- Arrow direction, tail shape, and zoom rotation must come from a consistent smooth model.
