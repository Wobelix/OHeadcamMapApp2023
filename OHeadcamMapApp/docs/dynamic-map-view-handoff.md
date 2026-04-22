# Dynamic Map View Handoff

Denne fil er skrevet som startkontekst til en ny Codex-session om en ny map overlay-visningstype i Headcam appen.

## Formaal

Appen har i dag to aktive map overlay-visninger:

- Zoom: et zoomet kortudsnit omkring loeberens aktuelle position.
- Leg: et laengere kortudsnit/strip for den aktuelle straekning.

Opgaven er at lave en tredje visningstype, "dynamic view", som minder om zoom/leg, men er mere dynamisk. Den praecise adfaerd er ikke besluttet endnu.

## Vigtige filer

- `MainForm.vb`
  - Orkestrerer render-flowet.
  - `MakeSmoothMapVideoAssets()` opretter zoom- og leg-overlay-videoer.
  - `RunMakeVideo()` kombinerer inputvideo og overlay-videoer.

- `clsMapRenderEngine.vb`
  - Aktiv render-engine for smooth overlay-videoer.
  - Har metoder for zoom og leg:
    - `RenderZoomFrame(...)`
    - `RenderZoomFramePerf8(...)`
    - `RenderLegFrame(...)`
    - `RenderLegFramePerf8(...)`
    - `WriteZoomVideoPerf8(...)`
    - `WriteLegVideoPerf8(...)`
  - Wrapper en legacy renderer: `_legacyRenderer As clsMapImages`.

- `clsMapImages.vb`
  - Legacy map renderer og settings-loader.
  - Indeholder mange grundindstillinger:
    - `ZoomWidth`, `ZoomHeight`, `ZoomRad`, `ZoomZoom`
    - `LegWidth`, `LegHeight`, `LegMargin`, `LegRad`
    - `dotSize`, `ArrowBarb`, `ArrowWidth`, tail/dot/frame settings
  - Indeholder gamle renderfunktioner:
    - `ZoomImageSmooth(...)`
    - `LapImageSmooth(...)`
    - `ZoomImage3Smooth(...)`
    - `LapImage2Smooth(...)`

- `clsExtra.vb`
  - Bygger ffmpeg-filteret til at laegge overlay-videoer oven paa inputvideoen.
  - `FFMPeg_MakeParamSmoothMapVideosOnVideo(...)` bruger pt. zoom- og leg-videoer som separate inputs.

## Nuværende flow

1. Brugeren vaelger inputvideo(er).
2. Appen joiner/klargoer inputvideo.
3. `MakeSmoothMapVideoAssets()` laeser QR XML + kortbillede.
4. Der oprettes en `clsMapRenderEngine` for zoom, hvis `My.Settings.cbShowRoute` er sand.
5. Der oprettes en `clsMapRenderEngine` for leg, hvis `My.Settings.cbShowLegMAp` er sand.
6. Zoom og leg renderes typisk til separate videoer:
   - `temp_smooth/map_overlay_z.mp4` eller alpha-format afhængigt af frame feather.
   - `temp_smooth/map_overlay_l.mp4` eller alpha-format afhængigt af frame feather.
7. `RunMakeVideo()` kalder ffmpeg via `FFMPeg_MakeParamSmoothMapVideosOnVideo(...)`.
8. Ffmpeg laegger overlay-videoerne paa inputvideoen med positioner fra:
   - `My.Settings.MIZoomMapPos`
   - `My.Settings.MILegMapPos`

## Outputformat

Outputlogikken er lige blevet aendret:

- `My.Settings.OutputFormat` styrer `Auto`, `Max HD 1920x1080`, `Max 2K 2560x1440`, `Max 4K 3840x2160`.
- Output er altid 16:9.
- `Auto` vaelger stoerste standardformat uden at opskalere inputbredden:
  - 1920x1080
  - 2560x1440
  - 3840x2160
- `chkHDFormat` er gammel UI/settings-rest og skal ikke bruges til ny logik.

## Afklarede designbeslutninger

- Ny scaling af alle map-objekter skal ikke laves nu.
- Pil, hale, dot og andre objekter paa selve kortet skal som udgangspunkt bevare deres dimensioner relativt til kortet.
- Hvis der senere laves output-oploesningsskalering, er det primært disse ting der skal overvejes:
  - bredde/hoejde paa zoom overlay
  - bredde/hoejde paa leg overlay
  - frame width / frame feather / rammeeffekt
  - eventuelt overlay-positioner paa slutvideoen
- Undgaa at skalere pil, hale, barb og dot automatisk med outputvideoens oploesning.

## Zoom-visningen i dag

Zoom bygger et kortudsnit omkring nuvaerende position.

Vigtige begreber:

- Outputstoerrelse: `_legacyRenderer.ZoomWidth`, `_legacyRenderer.ZoomHeight`
- Afrunding/frame: `_legacyRenderer.ZoomRad`, `_legacyRenderer.FrameFeather`
- Zoomniveau: `_legacyRenderer.ZoomZoom`
- Midtpunkt: route point ved aktuel tid
- Retning: typisk heading + 180 grader
- Resultatet skrives som en video og placeres senere paa slutvideoen.

Relevante metoder:

- `clsMapRenderEngine.RenderZoomFrame(...)`
- `clsMapRenderEngine.RenderZoomFramePerf8(...)`
- `clsMapRenderEngine.WriteZoomVideoPerf8(...)`

## Leg-visningen i dag

Leg bygger et laengere udsnit for aktuel straekning.

Vigtige begreber:

- Outputstoerrelse: `_legacyRenderer.LegWidth`, `_legacyRenderer.LegHeight`
- Margin: `_legacyRenderer.LegMargin`
- Afrunding/frame: `_legacyRenderer.LegRad`, `_legacyRenderer.FrameFeather`
- `BuildLegRenderInfo(...)` samler geometri og cache-noegler.
- Der er flere performance-varianter, men `Perf8` er aktivt brugt i main flowet.

Relevante metoder:

- `clsMapRenderEngine.BuildLegRenderInfo(...)`
- `clsMapRenderEngine.RenderLegFramePerf8(...)`
- `clsMapRenderEngine.WriteLegVideoPerf8(...)`

## Sandsynlig implementationsstrategi for ny dynamic view

Start helst som en tredje parallel overlay-video, ikke som en stor omskrivning af zoom/leg.

Mulige trin:

1. Tilfoej nye settings:
   - show/hide dynamic view
   - dynamic width/height
   - dynamic position paa slutvideo
   - eventuelt dynamic mode/style
2. Tilfoej properties i `clsMapRenderEngine`, hvis viewet skal have egne dimensioner.
3. Tilfoej en renderfunktion:
   - `RenderDynamicFrame(...)`
   - evt. senere `RenderDynamicFramePerf8(...)`
4. Tilfoej writer:
   - `WriteDynamicVideoPerf8(...)`
5. Udvid `MainForm.MakeSmoothMapVideoAssets()` til at generere dynamic overlay-videoen.
6. Udvid `clsExtra.FFMPeg_MakeParamSmoothMapVideosOnVideo(...)` eller lav en ny variant, saa ffmpeg kan tage tredje overlay input.
7. Justeringsvindue/UI kan derefter faa kontrol af dynamic overlayets stoerrelse og position.

## Vigtige faldgruber

- Undgaa at blande "kortets egen skala" med "videoens outputskala".
- Undgaa dobbelt-skalering af settings.
- Bevar zoom/leg som de virker nu, og tilfoej dynamic som en smal parallel feature.
- Husk at overlay-videoer kan vaere `.mp4` med colorkey eller alpha-format afhængigt af frame feather.
- Husk at `MakeSmoothMapVideoAssets()` kan renderere zoom og leg parallelt; dynamic skal enten ind i samme parallel-model eller koeres sekventielt i foerste version.

## God startprompt til ny session

Laes `docs/dynamic-map-view-handoff.md`. Jeg vil lave en tredje map overlay-visningstype i Headcam appen, parallelt med zoom og leg. Start med at inspicere `clsMapRenderEngine.vb`, `MainForm.MakeSmoothMapVideoAssets()` og `clsExtra.FFMPeg_MakeParamSmoothMapVideosOnVideo(...)`, og foreslaa den mindst risikable implementationsplan.
