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
- Smooth heading and tail smoothing have been improved enough for practical testing.
- Smooth overlay asset generation now runs through `clsMapRenderEngine.vb`.
- Final FFmpeg overlay now uses smooth zoom/leg videos in the normal path.
- Legacy `temp3\\%08d.png` generation is kept only as commented reference, not as the active path.

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
- `clsMapRenderEngine.vb`
- `MainForm.vb`

Tasks:
- Generate standalone smooth overlay videos for zoom and leg.
- Keep frame step and fps derived consistently.
- Compare render-engine timing with the earlier bitmap-heavy smooth path.

Tests:
- Verify output video length matches requested duration.
- Verify first/middle/last frames against preview.
- Verify both videos stay in sync with each other.
- Verify transparency works via key color without removing real black map details.

### 3. Final FFmpeg Overlay Path
Files:
- `MainForm.vb`
- `clsExtra.vb`

Tasks:
- Use smooth overlay videos instead of `temp3` image sequences.
- Overlay the zoom/leg videos at the stored positions and sizes.
- Keep enough legacy code in comments/reference form to recover behavior if needed.

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

## Recent Findings
- Feathered frame masking was a major performance bottleneck in the old smooth path.
- The render-engine `leg`/`zoom` test path is substantially faster than the earlier smooth asset generator.
- 4K output works, but should be validated with short clips because final encoding is still expensive.

## Notes
- The adjustment window is the visual truth source. Final render should match it.
- Time step in smooth mode should be expressed in seconds, not primarily as fps.
- Arrow direction, tail shape, and zoom rotation must come from a consistent smooth model.
