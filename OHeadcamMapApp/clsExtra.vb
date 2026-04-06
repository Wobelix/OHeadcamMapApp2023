Imports System.Globalization
Imports System.IO
Imports System.Text
Imports LibVLCSharp.Shared
Imports OHeadcamMapApp.Namespace_XFFMPG

Namespace Namespace_XFFMPG
    Public Enum OverlayPosition
        TopRight
        TopLeft
        TopCenter
        BottomRight
        BottomLeft
        BottomCenter
        CenterCenter
        CenterRight
        CenterLeft
    End Enum
End Namespace
Public Class clsExtra
    Public AppFolder As String
    Public BlackFolder As String = "temp1\"
    Public MapFolder As String = "temp2\"
    Public JoinListFile As String = "joinlist.txt"
    Public NoOfFrames As Integer
    Public PANEL_X As String = 880
    Public PANEL_Y As String = 632
    Public PANEL_WIDTH As String = 1280 - PANEL_X
    Public PANEL_HEIGHT As String = 720 - PANEL_Y
    Public GRAPH_X As String = 245
    Public GRAPH_Y As String = 631
    Public GRAPH_WIDTH As String = 625
    Public GRAPH_HEIGHT As String = 720 - GRAPH_Y
    Public MAPS_MARG As Integer = 4
    Public LEGMAP_WIDTH As String = 225 - 2 * MAPS_MARG
    Public LEGMAP_HEIGHT As String = 400 - 2 * MAPS_MARG
    Public LEGMAP_X As String = 10 + MAPS_MARG
    Public LEGMAP_Y As String = 310 + MAPS_MARG
    Public MAP_X As String = 1040 + MAPS_MARG
    Public MAP_Y As String = 310 + MAPS_MARG
    Public MAP_WIDTH As String = 225 - 2 * MAPS_MARG
    Public MAP_HEIGHT As String = 285 - 2 * MAPS_MARG
    Public MAP_FPS As String = "1"
    Public FFMPEG_INPUT_PARAM As String = " -r 25"
    Public FFMPEG_MAP_PARAM As String = " -map " + """" + "[out]" + """"
    Public Language As String = "da-DK"
    Public InputWidth As Integer = 1920
    Public InputHeight As Integer = 1080
    Public Sub New()
        AppFolder = My.Application.Info.DirectoryPath + "\"

    End Sub
    Function FFMpeg_VideoFrameToImage(VideoFile As String, ImageFile As String, Seconds As String) As String
        Return "-ss " + Seconds + " -i " + VideoFile + " -frames:v 1 -y " + ImageFile

    End Function
    Function MakeBlackFrameFiles(iNoOfFrames As Integer) As Boolean
        Dim f As String
        My.Computer.FileSystem.DeleteDirectory(AppFolder + BlackFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
        My.Computer.FileSystem.CreateDirectory(AppFolder + BlackFolder)
        NoOfFrames = iNoOfFrames
        For i = 0 To NoOfFrames
            f = "sort1080x720.jpg"
            'f = "pink1080x720.jpg"
            My.Computer.FileSystem.CopyFile(AppFolder + "layout_default\" + f, AppFolder + BlackFolder + i.ToString("D8") + ".jpg", True)
        Next i
        Return True
    End Function

    Function FF_Rotatefilter(ClockWDegrees As Single, VideoWidth As Integer, VideoHeight As Integer, Crop As Single) As String
        Dim Filter As String
        Dim Rot As String
        Dim CVal As String
        CVal = Crop.ToString("F2", CultureInfo.InvariantCulture)
        Rot = ""
        If ClockWDegrees <> 0 Then
            Rot = "rotate=" + ClockWDegrees.ToString("F1", CultureInfo.InvariantCulture) + "*PI/180,"
        End If
        Filter = Rot + "crop=" + CVal + "*in_w:" + CVal + "*in_h,scale=" + VideoWidth.ToString + ":" + VideoHeight.ToString
        Return Filter
    End Function
    Function FF_ColorImprovefilter(Optional strength As String = "0.5") As String
        Return "normalize=blackpt=black:whitept=white:smoothing=10:strength=" + strength
    End Function

    Function FF_LensCorrection() As String
        Return "lenscorrection=cx=0.5:cy=0.5:k1=-0.200:k2=0.000"
    End Function
    'brightness/lysstyrke:(0.0) The brightness parameter adjusts the overall brightness Of the image. It typically ranges from -1.0 (maximum decrease In brightness) To 1.0 (maximum increase In brightness). You can go beyond this range If you need more extreme adjustments, but keep In mind that going too far may result In loss Of details Or unnatural-looking images.
    'contrast/kontrast:(1.0) The contrast parameter controls the difference between the light And dark areas Of the image. 
    'hue/nuance:(0) The hue parameter shifts the hue Or color tint Of the image. It typically ranges from -180 To 180, representing the degrees Of hue rotation. Negative values shift the hue towards the blue-green spectrum, While positive values shift it towards the red-yellow spectrum.
    'saturation/mætning:(1.0) The saturation parameter controls the intensity Or richness Of colors In the image. It usually ranges from 0.0 (desaturated Or grayscale) To 2.0 (maximum saturation increase). Values beyond 2.0 can result In overly saturated Or unrealistic colors.
    'gamma/gamma:(1.0) The gamma parameter adjusts the gamma correction Of the image. It typically ranges from 0.1 To 10.0. Lower values (<1.0) darken the midtones And brighten the highlights, While higher values (>1.0) brighten the midtones And darken the highlights.
    '    contrast
    'Set the contrast expression. The value must be a float value In range -1000.0 To 1000.0. The Default value Is "1".

    'brightness
    'Set the brightness expression. The value must be a float value In range -1.0 To 1.0. The Default value Is "0".

    'saturation
    'Set the saturation expression. The value must be a float In range 0.0 To 3.0. The Default value Is "1".
    Function FF_ImageEQFilter(brightness As Single, contrast As Single, saturation As Single, gamma As Single) As String
        Return "eq=brightness=" + brightness.ToString("F2", CultureInfo.InvariantCulture) + ":contrast=" + contrast.ToString("F2", CultureInfo.InvariantCulture) +
            ":saturation=" + saturation.ToString("F2", CultureInfo.InvariantCulture) + ":gamma=" + gamma.ToString("F2", CultureInfo.InvariantCulture) '"eq=brightness=0.2:contrast=1.5:saturation=1.2" + 
    End Function
    'h
    'Specify the hue angle As a number Of degrees. It accepts an expression, And defaults To "0".

    's
    'Specify the saturation In the [-10,10] range. It accepts an expression And defaults To "1".
    Function FF_ImageHueFilter(hue As Single) As String
        Return "hue=H=" + hue.ToString("F0", CultureInfo.InvariantCulture)
    End Function

    'luma_spatial: This parameter controls the strength Of luma (brightness) spatial noise reduction. Higher values increase the strength Of noise reduction For luma. A typical range For this parameter Is 0 To 50. Values closer To 0 preserve more detail but may retain more noise, While higher values result In stronger noise reduction but may blur the image.

    'luma_tmp: This parameter determines the strength Of luma temporal noise reduction. It reduces noise by analyzing information across multiple frames. Like luma_spatial, a typical range Is 0 To 50. A value Of 0 disables temporal noise reduction, While higher values increase the strength.

    'chroma_spatial: This parameter controls the strength Of chroma (color) spatial noise reduction. It Is similar To luma_spatial but applies To chroma channels. A typical range Is 0 To 50.

    'chroma_tmp: This parameter determines the strength Of chroma temporal noise reduction. It Is similar To luma_tmp but applies To chroma channels. A typical range Is 0 To 50.

    'You can experiment With different values For these parameters To achieve the desired balance between noise reduction And preservation Of image details. It Is recommended To test different combinations And preview the results To find the optimal settings For your specific video sequences.

    Function FF_DenoiseFilter(Luma_s As Integer, luma_t As Integer, Chroma_s As Integer, Chroma_t As Integer) As String
        Return "hqdn3d=4:3:16:3" ' i parameterrækkefølge
    End Function
    'lx: Specifies the strength Of the sharpening effect. Higher values result In more sharpening. For example, 5 Is a commonly used value.
    'ly: Specifies the amount Of horizontal sharpening. The Default value Is the same As lx, so you can use the same value For both.
    'la: Specifies the strength Of the antiringing effect. This parameter helps reduce the halos that can sometimes appear around sharpened edges. A value Of 1.0 Is a good starting point.
    Function FF_SharpFilter(Optional strength As Integer = 5) As String
        Dim l = strength.ToString
        Dim SharpParam As String = l + ":" + l + ":1.0:" + l + ":" + l + ":0.0"
        Return "unsharp=" + SharpParam
    End Function
    Function FFMpeg_Crop(width As Integer, height As Integer, x As Integer, y As Integer) As String
        FFMpeg_Crop = "crop=" + width.ToString + ":" + height.ToString + ":" + x.ToString + ":" + y.ToString
    End Function
    Function FFMpeg_Overlay(margin_horizontal As Integer, margin_vertical As Integer, position As String) As String
        Dim OverlayPosStrTopL, OverlayPosStrTopR, OverlayPosStrBotL, OverlayPosStrBotR, OverlayPosStr, MarginH, MarginV As String


        MarginH = Convert.ToString(margin_horizontal)
        MarginV = Convert.ToString(margin_vertical)
        OverlayPosStrTopL = MarginH + "" + MarginV
        OverlayPosStrBotL = MarginH + "main_h-(overlay_h+" + MarginV + ")"
        OverlayPosStrTopR = "main_w-(overlay_w+" + MarginH + "):" + MarginV
        OverlayPosStrBotR = "main_w-(overlay_w+" + MarginH + ")main_h-(overlay_h+" + MarginV + ")"
        OverlayPosStr = OverlayPosStrBotR
        Select Case position
            Case "Top Left"
                OverlayPosStr = OverlayPosStrTopL
            Case "Top Right"
                OverlayPosStr = OverlayPosStrTopR
            Case "Bottom Left"
                OverlayPosStr = OverlayPosStrBotL
            Case "Bottom Right"
                OverlayPosStr = OverlayPosStrBotR

        End Select
        FFMpeg_Overlay = "overlay=" + OverlayPosStr
    End Function
    Function FFMpeg_ScalePadFHD(Optional Vid_width As Integer = -1, Optional Vid_Height As Integer = -1, Optional PaddingSide As String = "X") As String
        Dim side, scale, sOutW, sOutH As String
        Dim AR_horis As Boolean
        Dim AR_in, AR_out As Single

        If Vid_width = 1920 And Vid_Height = 1080 Then Return "" ' no scale/padding


        AR_out = 1920 / 1080
        If Vid_width = -1 Then
            AR_in = AR_out
        Else
            AR_in = Vid_width / Vid_Height
        End If
        '2.7K and 4K handling 
        'sOutW = CStr(InputWidth)
        sOutW = CStr((CInt(Math.Round(InputWidth)) \ 2) * 2) ' lige tal
        sOutH = CStr((CInt(Math.Round(CInt(sOutW) / 16 * 9)) \ 2) * 2) ' lige tal
        'sOutH = CStr(Math.Round(InputWidth / 16 * 9))

        If AR_in > AR_out Then
            scale = "scale=1920:-1" ' vertical padding
            AR_horis = False
        Else ' most used for 4:3 videos

            If Vid_width = -1 Then

                scale = $"scale={sOutW}:{sOutH}:force_original_aspect_ratio=decrease" ' should handle 2.7k and 4:3
                'scale = "scale=1920:1080:force_original_aspect_ratio=decrease" ' should handle 2.7k and 4:3
            Else
                scale = "scale=-1:1080" ' both up and downscale
            End If
            AR_horis = True
        End If


        If Not PaddingSide = "X" Then
                side = PaddingSide
            Else
                side = My.Settings.VideoPadding
            End If
            Select Case side
                Case "L"
                FFMpeg_ScalePadFHD = $"{scale},pad={sOutW}:{sOutH}:(ow-iw):ceil((oh-ih)/2):color=black"
            Case "R"
                FFMpeg_ScalePadFHD = $"{scale},pad={sOutW}:{sOutH}:0:ceil((oh-ih)/2):color=black"
            Case Else
                FFMpeg_ScalePadFHD = $"{scale},pad={sOutW}:{sOutH}:ceil((ow-iw)/2):ceil((oh-ih)/2):color=black"
        End Select


    End Function
    Function FFMpeg_Scale(Scale As Decimal) As String
        Dim strsc As String
        strsc = Scale.ToString("F1")
        strsc = Replace(strsc, ",", ".")
        FFMpeg_Scale = "scale=trunc(iw*" + strsc + "/2)*2" + "trunc(ih*" + strsc + "/2)*2"
    End Function
    Function FFMpeg_VideoMap_FilterOverlays(videoinput As String, mapinput As String, VideoMapSpeed As String) As String
        Dim inputscale, strscale, omap, fInput, fFilters, transparency, SngSpeed, Speedstr, Speedparam As String
        transparency = My.Settings.Transparency
        transparency = transparency.Replace(",", ".")
        inputscale = videoinput + FFMpeg_ScalePadFHD() 'videopadding
        strscale = "," + FFMpeg_Scale(My.Settings.dFfmpegScaleMap)

        'SngTempo = Decimal.ToSingle(Tempo)
        SngSpeed = Decimal.ToSingle(VideoMapSpeed) '1 / SngTempo
        Speedstr = SngSpeed.ToString
        Speedstr = Speedstr.Replace(",", ".")
        If SngSpeed <> 1 Then
            Speedparam = ",setpts=" + Speedstr + "*PTS"
        Else
            Speedparam = ""
        End If

        omap = FFMpeg_Overlay(CInt(My.Settings.MRouteH), CInt(My.Settings.MRouteV), "Bottom_Right")
        fInput = inputscale + "[si1];"    '[Si1]=inputvideo
        'fnexti = "[si1]"
        fFilters = fInput + mapinput + "format=rgba,colorchannelmixer=aa=" + transparency + Speedparam + strscale + "[tout];"
        fFilters += "[si1][tout]" + omap
        Return fFilters
    End Function
    Function NoOfPanels() As Integer
        NoOfPanels = 0
        If My.Settings.cbShowRoute Then NoOfPanels += 1 'routemap
        If My.Settings.cbShowLegMAp Then NoOfPanels += 1 'legmap
        If My.Settings.cbHeightGraph Then NoOfPanels += 1 'Graph
        If My.Settings.cbShowSpeedPanel Then NoOfPanels += 1
    End Function
    Function FF_SplitStr() As String
        FF_SplitStr = ""
        If NoOfPanels() > 0 Then
            FF_SplitStr = "split=" + NoOfPanels.ToString
            For i = 1 To NoOfPanels()
                FF_SplitStr += "[img" + i.ToString + "]"
            Next i
        End If
    End Function
    Function FF_ImgToVideo(ImgProp As clsFFMPegProbe, Duration As String, outPath As String, Optional DoPad As Boolean = True, Optional Tag As String = "") As String 'keep ext and add .mp4 to outp
        Dim d, outfile, param As String
        d = TimeStrToSec(Duration).ToString
        outfile = ChangeFileExtension(ImgProp.Videofilename, "mp4", Tag, outPath)

        param = " -loop 1 -i " + """" + ImgProp.Videofilename + """"
        If DoPad Then
            If Not (ImgProp.Width = 1920 And ImgProp.Height = 1080) Then
                param += " -vf " + FFMpeg_ScalePadFHD(ImgProp.Width, ImgProp.Height, "C") ' center padding
            End If
        Else
            param += " -vf scale=w=trunc(iw/2)*2:h=trunc(ih/2)*2" 'Secure against uneven width or height - gives error in libx264
        End If
        param += FFMPeg_MakeOutputStr(outfile, d, False, False)
        Return param
    End Function

    Function ChangeFileExtension(fullPath As String, newExtension As String, Optional Tag As String = "", Optional outPath As String = Nothing) As String
        Dim directory As String
        If String.IsNullOrEmpty(outPath) Then
            directory = Path.GetDirectoryName(fullPath)
        Else
            directory = outPath
        End If

        'Dim filenameWithoutExtension As String = Path.GetFileNameWithoutExtension(fullPath)
        'Dim ext As String = Path.GetExtension(fullPath)

        Dim newFilename As String = fullPath.Replace(".", "") + Tag + "." + newExtension

        Return Path.Combine(directory, newFilename)

    End Function
    Function FF_audio_input(Videofiles_I As List(Of clsFFMPegProbe), Props As List(Of clsPostFProps)) As String
        Dim out As String = ""
        For i = 0 To Videofiles_I.Count - 1

            If Props(i).bIsImage Or Not Props(i).bHasAudio Then ' images and videos without audio
                out += $" -f lavfi -t {Props(i).iDuration} -i anullsrc=r=44100:cl=stereo" 'silence for images
            Else
                fn = Videofiles_I(i).Videofilename
                out += $" -i ""{fn}"""
            End If
        Next i
        Return out
    End Function
    Function FF_audio_afade(VideoPropList As List(Of clsPostFProps)) As String 'out is no []
        Dim out As String = ""
        Dim Param As String
        Dim DurSum As Integer = 0
        If VideoPropList.Count > 1 Then ' first video is not using fade
            DurSum = VideoPropList(0).iDuration - CInt(VideoPropList(1).sTDuration) ' fade start
            Param = $"acrossfade=duration={VideoPropList(1).sTDuration}"
            out = "[0:a][1:a]" + Param + "[fade1];" 'PropList(0) is the for the first video 

            For i = 2 To VideoPropList.Count - 1
                DurSum += VideoPropList(i - 1).iDuration - CInt(VideoPropList(i).sTDuration) 'End of first mix
                Param = $"acrossfade=duration={VideoPropList(i).sTDuration}"
                out += $"[fade{(i - 1)}][{i}]{Param}[fade{i}];"
            Next i
            out = out.Substring(0, out.Length - 1) 'remove ;
            out = out.Replace($"[fade{(VideoPropList.Count - 1)}]", "") ' last one
        End If

        Return out
    End Function
    Function FF_movie_input(Videofiles_I As List(Of clsFFMPegProbe), Props As List(Of clsPostFProps)) As String
        Dim out As String
        out = ""
        fps = ""
        For i = 0 To Videofiles_I.Count - 1
            If Not Videofiles_I(i).framerate = 25 Then ' alle filer skal have det for at fade virker
                fps = ",fps=25"
            End If
        Next
        For i = 0 To Videofiles_I.Count - 1
            fn = FFPlay_Filename(Videofiles_I(i).Videofilename)
            out += "movie=" + fn
            If Props(i).bIsImage Then
                out += $":loop={Props(i).iDuration},setpts=N/25/TB" '1 sec video for images
            Else
                out += fps
            End If
            If Not (Videofiles_I(i).Width = 1920 And Videofiles_I(i).Height = 1080) Then
                out += "," + FFMpeg_ScalePadFHD(Videofiles_I(i).Width, Videofiles_I(i).Height, "C") 'centerpadding
            End If
            out += $"[vid{i}];"
        Next
        Dim out2 As String = out.Substring(0, out.Length - 1) 'remove ;
        Return out2
    End Function
    Function FF_movie_xfade(VideoPropList As List(Of clsPostFProps)) As String 'out is [fadeout]
        Dim out As String = ""
        Dim Param As String
        Dim DurSum As Integer = 0
        If VideoPropList.Count > 1 Then ' first video is not using fade
            out = ""
            For i = 0 To VideoPropList.Count - 1
                out += $"[vid{i}]settb=AVTB,setpts=PTS-STARTPTS[vidx{i}];" 'avoid time base problem
            Next
            DurSum = VideoPropList(0).iDuration - CInt(VideoPropList(1).sTDuration) ' fade start
            Param = "xfade=transition=" + VideoPropList(1).TTransition + ":duration=" + VideoPropList(1).sTDuration + ":offset=" + DurSum.ToString
            out += "[vidx0][vidx1]" + Param + "[fade1];" 'PropList(0) is the for the first video 

            For i = 2 To VideoPropList.Count - 1
                DurSum += VideoPropList(i - 1).iDuration - CInt(VideoPropList(i).sTDuration) 'End of first mix
                Param = "xfade=transition=" + VideoPropList(i).TTransition + ":duration=" + VideoPropList(i).sTDuration + ":offset=" + DurSum.ToString
                out += "[fade" + (i - 1).ToString + "][vidx" + i.ToString + "]" + Param + "[fade" + i.ToString + "];"
            Next i
        End If

        Dim out2 As String = out.Substring(0, out.Length - 1) 'remove ;
        out2 = out2.Replace("[fade" + (VideoPropList.Count - 1).ToString + "]", "[fadeout]") ' last one
        Return out2
    End Function

    Function FF_movie_overlays(InputTag As String, MediaList As List(Of clsFFMPegProbe), OverlayList As List(Of clsPostOverlay)) As String 'out Is [overout]
        Dim overlayPositionFilter As String = "", output, PrevTag, CurTag As String
        Try
            PrevTag = InputTag
            output = ""
            For i = 0 To OverlayList.Count - 1
                Dim ol As clsPostOverlay = OverlayList(i)
                Dim m As clsFFMPegProbe = MediaList(i)
                Dim scalestr, w, h, x, y As String
                w = ol.sSWidth
                h = ol.sSHeight
                If ol.sSWidth = "" Or ol.sSWidth = "-1" Then
                    w = "-1"
                End If
                If ol.sSHeight = "" Or ol.sSHeight = "-1" Then
                    h = "-1"
                End If
                If Not (w = "-1" And h = "-1") Then
                    scalestr = $",scale={w}:{h}"
                Else
                    scalestr = ""
                End If
                If IsNumeric(ol.sX) Then
                    x = ol.sX
                Else
                    x = "0"
                End If
                If IsNumeric(ol.sY) Then
                    y = ol.sY
                Else
                    y = "0"
                End If
                fd = CInt(1)
                f_st = CInt(ol.sDuration) - fd

                fadestr = $",fade=t=in:st=0:d=2:alpha=1,fade=t=out:st={f_st}:d={fd}:alpha=1,setpts=PTS-STARTPTS+{ol.sStart}/TB" '+12 er start ifht til total video
                CurTag = $"[ovr{i}]"
                output += $";movie={FFPlay_Filename(m.Videofilename)}"
                If ol.bIsImage Then
                    output += $":loop={ol.sDuration},setpts=N/25/TB"
                End If
                output += $"{scalestr}{fadestr}"
                output += $"{CurTag}"
                overlayPositionFilter = $";{PrevTag}{CurTag}"
                Select Case ol.PlacementT
                    Case OverlayPosition.TopRight
                        overlayPositionFilter += $"overlay=x={x}+main_w-overlay_w:y={y}"
                    Case OverlayPosition.TopLeft
                        overlayPositionFilter += $"overlay=x={x}:y={y}"
                    Case OverlayPosition.TopCenter
                        overlayPositionFilter += $"overlay=x={x}+(main_w-overlay_w)/2:y={y}"
                    Case OverlayPosition.BottomRight
                        overlayPositionFilter += $"overlay=x={x}+main_w-overlay_w:y={y}+main_h-overlay_h"
                    Case OverlayPosition.BottomLeft
                        overlayPositionFilter += $"overlay=x={x}:y={y}+main_h-overlay_h"
                    Case OverlayPosition.BottomCenter
                        overlayPositionFilter += $"overlay=x={x}+(main_w-overlay_w)/2:y={y}+main_h-overlay_h"
                    Case OverlayPosition.CenterCenter
                        overlayPositionFilter += $"overlay=x={x}+(main_w-overlay_w)/2:y={y}+(main_h-overlay_h)/2"
                    Case OverlayPosition.CenterRight
                        overlayPositionFilter += $"overlay=x=main_w-overlay_w+{x}:y={y}+(main_h-overlay_h)/2"
                    Case OverlayPosition.CenterLeft
                        overlayPositionFilter += $"overlay=x={x}:y={y}+(main_h-overlay_h)/2"
                    Case Else
                        ' Default to CenterCenter
                        overlayPositionFilter += $"overlay=x=(main_w-overlay_w)/2:y=(main_h-overlay_h)/2"
                End Select
                Endtime = (CInt(ol.sStart) + CInt(ol.sDuration)).ToString
                output += $"{overlayPositionFilter}:enable='between(t,{ol.sStart},{Endtime}')[oout{i}]"
                PrevTag = $"[oout{i}]"
            Next
            output = output.Replace(PrevTag, "[overout]")
        Catch ex As Exception
            MsgBox(ex.Message)
            output = ""
        End Try
        Return output

    End Function
    Function FF_movie_xfade1(VideoCount As Integer, duration As Integer, offset As Integer, Optional trans As String = "fade") As String 'out is [fade<VideoCount-1>]
        Dim out As String = ""
        Dim Param As String
        Param = "xfade=transition=" + trans + ":duration=" + duration.ToString + ":offset=" + offset.ToString
        If VideoCount > 1 Then
            out = "[vid0][vid1]" + Param + "[fade1];"

            For i = 2 To VideoCount - 1

                out += "[fade" + (i - 1).ToString + "][vid" + i.ToString + "]" + Param + "[fade" + i.ToString + "];"
            Next i
        End If
        Dim out2 As String = out.Substring(0, out.Length - 1) 'remove ;
        out2 = out2.Replace("[fade" + (VideoCount - 1).ToString + "]", "[fadeout]") ' last one
        Return out2
    End Function
    Function FFPlay_Filename(fn As String) As String
        Dim videofile_esc As String
        videofile_esc = "'" + fn.Replace("\", "\/") + "'"
        videofile_esc = videofile_esc.Replace(":", "\:")
        videofile_esc = videofile_esc.Replace("""", "")
        Return videofile_esc
    End Function
    Function FF_MakeComplexinput(videofile As String, mapfile As String, VideoStart As Integer, MapStart As Integer, Optional IsFFPlay As Boolean = False) As String
        Dim videofile_esc, mapfile_esc, Result, Fstr, VideoStartStr, MapStartStr As String
        Dim i As Integer
        videofile_esc = "'" + videofile.Replace("\", "\/") + "'"
        videofile_esc = videofile_esc.Replace(":", "\:")
        videofile_esc = videofile_esc.Replace("""", "")
        mapfile_esc = "'" + mapfile.Replace("\", "\/") + "'"

        If IsFFPlay Then
            Fstr = "-f lavfi "
        Else
            Fstr = "-filter_complex "
        End If
        VideoStartStr = ""
        MapStartStr = ""
        If VideoStart > 0 Then
            VideoStartStr = ",setpts=PTS-STARTPTS+" + VideoStart + "/TB"
        End If
        If MapStart > 0 Then
            MapStartStr = ",select=gte(n\," + MapStart.ToString + ")"
        End If
        Result = Fstr + """" + "movie=" + videofile_esc + VideoStartStr + "[vf];movie=" + mapfile_esc + ",setpts=N/TB" + MapStartStr + "," + FF_SplitStr()

        Return Result
    End Function
    Function ScalePoint(InputP As Point, scale As Single) As Point
        Dim scaledPoint As New Point
        scaledPoint.X = CInt(Math.Round(InputP.X * scale))
        scaledPoint.Y = CInt(Math.Round(InputP.Y * scale))
        Return scaledPoint
    End Function
    Function FFMpeg_MakefilterCropsOverlays(videoinput As String) As String
        Dim cmapvid, clegmapvid, olegmap, omap, inputscale, mapscale, ograph, cgraph, cpanel, opanel, strscale, tmp, tmp2, transparency, transcolor, ckey, fInput, fFilters, fOutput, fnexti, n, MapWidth, MapHeight As String
        Dim scalesng, VideoScale As Single
        Dim ScaledZoomMapPos, ScaledLegMapPos As Point
        Dim ECount As Integer
        transparency = My.Settings.Transparency
        transparency = transparency.Replace(",", ".")
        transcolor = "0x000000"
        'transcolor = "0xFFAEC9" 'YUV"0xC97FA6" ' pink
        ckey = "colorkey=" + transcolor + ":0.01:0.5"

        MapWidth = MAP_WIDTH
        MapHeight = MAP_HEIGHT

        inputscale = videoinput + FFMpeg_ScalePadFHD() 'videopadding
        mapscale = "scale=1920:1080"
        strscale = FFMpeg_Scale(My.Settings.dFfmpegScaleMap)
        If My.Settings.cbNewMapF Then ' Mapfile format
            cmapvid = FFMpeg_Crop(My.Settings.MIFZoomDim.X, My.Settings.MIFZoomDim.Y, My.Settings.MIFZoomPos.X, My.Settings.MIFZoomPos.Y)
            clegmapvid = FFMpeg_Crop(My.Settings.MIFLegDim.X, My.Settings.MIFLegDim.Y, My.Settings.MIFLegPos.X, My.Settings.MIFLegPos.Y)
            'Scale
            'VideoScale = InputWidth / 1920 ' scale factor
            VideoScale = 1 'scale2
            ScaledZoomMapPos = ScalePoint(My.Settings.MIZoomMapPos, VideoScale)
            ScaledLegMapPos = ScalePoint(My.Settings.MILegMapPos, VideoScale)
            omap = "overlay=" + CStr(ScaledZoomMapPos.X) + ":" + CStr(ScaledZoomMapPos.Y)
            olegmap = "overlay=" + CStr(ScaledLegMapPos.X) + ":" + CStr(ScaledLegMapPos.Y)
        Else
            cmapvid = FFMpeg_Crop(MapWidth, MapHeight, MAP_X, MAP_Y) + "," + strscale
            clegmapvid = FFMpeg_Crop(LEGMAP_WIDTH, LEGMAP_HEIGHT, LEGMAP_X, LEGMAP_Y) + "," + strscale
            omap = FFMpeg_Overlay(CInt(My.Settings.MRouteH), CInt(My.Settings.MRouteV), "Bottom_Right")
            olegmap = FFMpeg_Overlay(CInt(My.Settings.MLegMapH), CInt(My.Settings.MLegMapV), "Bottom Left")
        End If
        cgraph = FFMpeg_Crop(GRAPH_WIDTH, GRAPH_HEIGHT, GRAPH_X, GRAPH_Y)
        cpanel = FFMpeg_Crop(PANEL_WIDTH, PANEL_HEIGHT, PANEL_X, PANEL_Y)
        'omap blev flyttet op over panel + margin


        'ograph flyttes til højre for legmap
        'tmp = My.Settings.ffmpegScaleMap.Split(".")(0)
        'tmp2 = My.Settings.ffmpegScaleMap.Split(".")(1)
        'scalesng = (CSng(tmp) + CSng(tmp2) / 10) * CSng(LEGMAP_WIDTH)
        ograph = FFMpeg_Overlay(CInt(My.Settings.MHeightH), CInt(My.Settings.MHeightV), "Bottom Left")
        opanel = FFMpeg_Overlay(CInt(My.Settings.MSpeedH), CInt(My.Settings.MSpeedV), "Bottom_Right")
        'FFMpeg_MakefilterCropsOverlays = inputscale + "[si1];" + mapinput + cmapvid + "[y1];[y1]format=rgba,colorchannelmixer=aa=" + transparency + "[c1];" + mapinput + clegmapvid + "[y2];[y2]format=rgba,colorchannelmixer=aa=" + transparency + "[c2];" + mapinput + cgraph + "[x3];[x3]" + ckey + "[y3];[y3]colorchannelmixer=aa=" + transparency + "[c3];" _
        '   + mapinput + cpanel + "[x4];[x4]" + ckey + "[y4];[y4]colorchannelmixer=aa=" + transparency + "[c4];[si1][c1]" + omap + "[o1];[o1][c2]" + olegmap + "[o2];[o2][c3]" + ograph + "[o3];[o3][c4]" + opanel
        'Ny metode
        ECount = 1
        fFilters = ""
        fInput = inputscale    '[Si1]=inputvideo
        fnexti = "[si1]"
        Dim cTmp As String
        If My.Settings.cbShowRoute Then 'routemap
            n = CStr(ECount)

            cTmp = cmapvid + ","

            fFilters = fnexti + ";" + "[img" + n + "]" + cTmp + "format=rgba,colorchannelmixer=aa=" + transparency + "[c" + n + "];"
            fFilters += fnexti + "[c" + n + "]" + omap
            fnexti = "[o" + n + "]" ';[o" + n + "]"
            ECount += 1
        End If
        If My.Settings.cbShowLegMAp Then 'legmap
            n = CStr(ECount)

            cTmp = clegmapvid + ","

            fFilters += fnexti + ";" + "[img" + n + "]" + cTmp + "format=rgba,colorchannelmixer=aa=" + transparency + "[c" + n + "];"
            fFilters += fnexti + "[c" + n + "]" + olegmap   'overlay legmap output
            fnexti = "[o" + n + "]" ';[o" + n + "]"
            ECount += 1
        End If
        If My.Settings.cbHeightGraph Then 'Graph
            n = CStr(ECount)
            fFilters += fnexti + ";" + "[img" + n + "]" + cgraph + "," + ckey + ",colorchannelmixer=aa=" + transparency + "[c" + n + "];"
            fFilters += fnexti + "[c" + n + "]" + ograph
            fnexti = "[o" + n + "]" '[o" + n + "]"
            ECount += 1
        End If
        If My.Settings.cbShowSpeedPanel Then 'Panel
            n = CStr(ECount)
            fFilters += fnexti + ";" + "[img" + n + "]" + cpanel + "," + ckey + ",colorchannelmixer=aa=" + transparency + "[c" + n + "];"
            fFilters += fnexti + "[c" + n + "]" + opanel 'previous + panel overlay
        End If
        If ECount = 1 Then
            'fInput += "[si1];"
        End If
        fOutput = fInput + fFilters
        'MsgBox(FFMpeg_MakefilterCropsOverlays + " : " + fOutput)
        Return fOutput
    End Function

    Function FFPlay_MakeParamMapOnVideo(mapfile As String, videofile As String, GPXDiff As String, Optional ScaleOut As String = "", Optional UseMapVideo As Boolean = False, Optional MapVideoSpeed As String = "1") As String
        Dim GPXd, InpD, OLength, Tempoparam, tempostr, videofile_esc, mapfile_esc As String
        Dim SngTempo, SngSpeed, tempo As Single
        Dim iGPXDiff, iVideoDiff, iMapDiff As Integer

        videofile_esc = "'" + videofile.Replace("\", "\/") + "'"
        videofile_esc = videofile_esc.Replace(":", "\:")
        videofile_esc = videofile_esc.Replace("""", "")
        mapfile_esc = "'" + mapfile.Replace("\", "\/") + "'"
        GPXd = ""
        InpD = ""
        OLength = ""

        If IsNumeric(GPXDiff) Then
            iGPXDiff = CInt(GPXDiff)
            If iGPXDiff > 0 Then
                iMapDiff = iGPXDiff
                iVideoDiff = 0

            End If
            If iGPXDiff < 0 Then
                iVideoDiff = Math.Abs(iGPXDiff)
                iMapDiff = 0
            End If
        End If
        SngTempo = Decimal.ToSingle(tempo)
        SngSpeed = 1 / SngTempo
        tempostr = SngSpeed.ToString
        tempostr = tempostr.Replace(",", ".")
        If SngSpeed <> 1 Then
            Tempoparam = "[cro];[cro]setpts=" + tempostr + "*PTS[out]"
        Else
            Tempoparam = "[out]"
        End If
        'FFMPEG_INPUT_PARAM +
        'Der skal bruges en img pr kortdel i ffplay: Input #0, lavfi, from 'movie='F\:\/Videoer\/TT280323\/deshaked.mp4'[vf];movie='temp2\/%08d.jpg',setpts=N/TB[img];movie='temp2\/%08d.jpg',setpts=N/TB[img2]

        If Not UseMapVideo Then

            FFPlay_MakeParamMapOnVideo = FF_MakeComplexinput(videofile, mapfile, iVideoDiff, iMapDiff, True) + ";" + FFMpeg_MakefilterCropsOverlays("[vf]") +
                ScaleOut + "[out]" + """"
        Else
            FFPlay_MakeParamMapOnVideo = InpD + " -i " + videofile + GPXd + " -i " + mapfile + " -filter_complex " + """" +
                FFMpeg_VideoMap_FilterOverlays("[0]", "[1]", MapVideoSpeed) + Tempoparam + "[out]" + """"
        End If

    End Function
    Function FFMPeg_MakeFilterParamMapOnVideo(mapfile As String, videofile As String, GPXDiff As String, Optional Length As String = "",
                                        Optional Tempo As Decimal = 1, Optional UseMapVideo As Boolean = False,
                                        Optional MapVideoSpeed As String = "1") As String
        Dim GPXd, InpD, OLength, Tempoparam, tempostr As String
        Dim SngTempo, SngSpeed As Single
        Dim iGPXDiff As Integer
        GPXd = ""
        InpD = ""
        OLength = ""
        If My.Settings.cbNewMapF Then
            My.Settings.cbHeightGraph = False
            My.Settings.cbShowSpeedPanel = False
            My.Settings.Save()
        End If

        If IsNumeric(GPXDiff) Then
            iGPXDiff = CInt(GPXDiff)
            If iGPXDiff > 0 Then
                If UseMapVideo Then
                    SngSpeed = Math.Abs(iGPXDiff) / MapVideoSpeed

                    GPXd = " -ss " + CStr(SngSpeed).Replace(",", ".")
                Else
                    GPXd = " -start_number " + GPXDiff
                End If
            End If
            If iGPXDiff < 0 Then

                InpD = " -ss " + CStr(Math.Abs(iGPXDiff))

            End If
        End If
        SngTempo = Decimal.ToSingle(Tempo)
        SngSpeed = 1 / SngTempo
        tempostr = SngSpeed.ToString
        tempostr = tempostr.Replace(",", ".")
        If SngSpeed <> 1 Then
            Tempoparam = "[cro];[cro]setpts=" + tempostr + "*PTS[out]"
        Else
            Tempoparam = "[out]"
        End If


        If Not UseMapVideo Then
            FFMPeg_MakeFilterParamMapOnVideo = InpD + " -i " + videofile + " -r " + MAP_FPS + GPXd + " -i " + mapfile + " -filter_complex " + """" +
                "[1]" + FF_SplitStr() + ";" + FFMpeg_MakefilterCropsOverlays("[0]") + Tempoparam + """"
        Else
            FFMPeg_MakeFilterParamMapOnVideo = InpD + " -i " + videofile + GPXd + " -i " + mapfile + " -filter_complex " + """" +
                FFMpeg_VideoMap_FilterOverlays("[0]", "[1]", MapVideoSpeed) + Tempoparam + """"
        End If

    End Function
    Function FFMPeg_MakeParamMapOnVideo(outputfile As String, mapfile As String, videofile As String, GPXDiff As String, Optional Length As String = "",
                                        Optional Tempo As Decimal = 1, Optional Quick As Boolean = False, Optional UseMapVideo As Boolean = False,
                                        Optional MapVideoSpeed As String = "1") As String
        Dim OutPStr, MapFilters As String

        If Quick Then
            OutPStr = FFMPeg_MakeQuickOutputStr(outputfile, Length)
        Else
            OutPStr = FFMPeg_MakeOutputStr(outputfile, Length)
        End If
        MapFilters = FFMPeg_MakeFilterParamMapOnVideo(mapfile, videofile, GPXDiff, Length,
                                         Tempo, UseMapVideo,
                                         MapVideoSpeed)

        FFMPeg_MakeParamMapOnVideo = MapFilters + FFMPEG_MAP_PARAM + OutPStr
        'FFMPeg_MakeParamMapOnVideo = InpD + " -i " + videofile + " -r " + MAP_FPS + GPXd + " -i " + mapfile + " -filter_complex " + """" +
        ' "[1]" + FF_SplitStr() + ";" + FFMpeg_MakefilterCropsOverlays("[0]") + Tempoparam + """" + FFMPEG_MAP_PARAM + OutPStr
        'Else
        'FFMPeg_MakeParamMapOnVideo = InpD + " -i " + videofile + GPXd + " -i " + mapfile + " -filter_complex " + """" _
        '  + FFMpeg_VideoMap_FilterOverlays("[0]", "[1]", MapVideoSpeed) + Tempoparam + """" + FFMPEG_MAP_PARAM + OutPStr
        'End If

    End Function
    Function FFMPeg_MakeParamSmoothMapVideosOnVideo(outputfile As String, zoomVideoFile As String, legVideoFile As String, videofile As String, GPXDiff As String, Optional Length As String = "",
                                                    Optional Tempo As Decimal = 1, Optional Quick As Boolean = False) As String
        Dim inputArgs As String = ""
        Dim filterParts As New List(Of String)
        Dim currentTag As String = "[si1]"
        Dim nextInputIndex As Integer = 1
        Dim currentOutputTag As String = "[cro]"
        Dim transparency As String = CStr(My.Settings.Transparency).Replace(",", ".")
        Dim keyColor As String = "0xFF00FF"
        Dim keyFilter As String = "colorkey=" & keyColor & ":0.01:0.5"
        Dim tmpGPXDiff As Integer = 0

        If IsNumeric(GPXDiff) Then tmpGPXDiff = CInt(GPXDiff)
        If tmpGPXDiff < 0 Then
            inputArgs = " -ss " + Math.Abs(tmpGPXDiff).ToString(CultureInfo.InvariantCulture)
        End If
        inputArgs += " -i " + """" + videofile + """"
        filterParts.Add("[0:v]" + FFMpeg_ScalePadFHD() + "[si1]")

        If My.Settings.cbShowRoute AndAlso File.Exists(zoomVideoFile) Then
            If tmpGPXDiff > 0 Then
                inputArgs += " -ss " + tmpGPXDiff.ToString(CultureInfo.InvariantCulture)
            End If
            inputArgs += " -i " + """" + zoomVideoFile + """"
            Dim zoomTag As String = $"[{nextInputIndex}:v]"
            filterParts.Add($"{zoomTag}format=rgba,{keyFilter},colorchannelmixer=aa={transparency}[c{nextInputIndex}]")
            filterParts.Add($"{currentTag}[c{nextInputIndex}]overlay={CStr(My.Settings.MIZoomMapPos.X)}:{CStr(My.Settings.MIZoomMapPos.Y)}[o{nextInputIndex}]")
            currentTag = $"[o{nextInputIndex}]"
            nextInputIndex += 1
        End If

        If My.Settings.cbShowLegMAp AndAlso File.Exists(legVideoFile) Then
            If tmpGPXDiff > 0 Then
                inputArgs += " -ss " + tmpGPXDiff.ToString(CultureInfo.InvariantCulture)
            End If
            inputArgs += " -i " + """" + legVideoFile + """"
            Dim legTag As String = $"[{nextInputIndex}:v]"
            filterParts.Add($"{legTag}format=rgba,{keyFilter},colorchannelmixer=aa={transparency}[c{nextInputIndex}]")
            filterParts.Add($"{currentTag}[c{nextInputIndex}]overlay={CStr(My.Settings.MILegMapPos.X)}:{CStr(My.Settings.MILegMapPos.Y)}[o{nextInputIndex}]")
            currentTag = $"[o{nextInputIndex}]"
            nextInputIndex += 1
        End If

        filterParts.Add($"{currentTag}null{currentOutputTag}")

        Dim tempoFilter As String = ""
        Dim sngTempo As Single = Decimal.ToSingle(Tempo)
        If sngTempo <> 1 Then
            Dim speed As String = (1 / sngTempo).ToString(CultureInfo.InvariantCulture)
            tempoFilter = $";{currentOutputTag}setpts={speed}*PTS[out]"
        Else
            tempoFilter = $";{currentOutputTag}null[out]"
        End If

        Dim outPStr As String
        If Quick Then
            outPStr = FFMPeg_MakeQuickOutputStr(outputfile, Length)
        Else
            outPStr = FFMPeg_MakeOutputStr(outputfile, Length)
        End If

        Return inputArgs + " -filter_complex " + """" + String.Join(";", filterParts) + tempoFilter + """" + FFMPEG_MAP_PARAM + outPStr
    End Function
    Function FFMPeg_MakeOutputStr(outputfile As String, Optional length As String = "", Optional CodecCopy As Boolean = False, Optional DoAudio As Boolean = True) As String
        Dim fps, crf, audio, preset, pix As String
        'presets: ultrafast,superfast,veryfast,faster,fast,medium,slow,slower,veryslow
        crf = My.Settings.ffmpegCRF
        fps = My.Settings.ffmpegOutFps
        If My.Settings.NoAudio Or Not DoAudio Then
            audio = ""
        Else
            audio = " -map 0:a -c:a aac -ac 2 -ar 48000 -b:a 128k"
        End If

        preset = My.Settings.ffmpegPreset
        If CodecCopy Then
            FFMPeg_MakeOutputStr = " -c:v copy -c:a aac -ac 2 -ar 48000 -b:a 128k"
            pix = ""
        Else ' HD, 2.7K, 4K
            If InputWidth < 2000 Then
                outputStr = " -c:v libx264 -crf " & crf & audio & " -r " & fps & " -preset " & preset
                pix = " -pix_fmt yuvj420p"
                FFMPeg_MakeOutputStr = outputStr & pix
            ElseIf InputWidth >= 2000 And OutputWidth <= 2800 Then '2.7K
                outputStr = " -c:v libx265 -crf " & crf & " -x265-params bitrate=50000" & audio & " -r " & fps & " -preset " & preset
                pix = " -pix_fmt yuv420p10le"
                FFMPeg_MakeOutputStr = outputStr & pix
            ElseIf InputWidth > 2800 Then '4K
                outputStr = " -c:v libx265 -crf " & crf & " -x265-params bitrate=90000" & audio & " -r " & fps & " -preset " & preset
                pix = " -pix_fmt yuv420p10le"
                FFMPeg_MakeOutputStr = outputStr & pix
            End If
        End If

        If length <> "" And length <> "0" Then
            FFMPeg_MakeOutputStr += " -t " + length
        End If

        FFMPeg_MakeOutputStr += " -y " + """" + outputfile + """"
    End Function
    Function FFMPeg_MakeQuickOutputStr(outputfile As String, Optional length As String = "", Optional DoAudio As Boolean = True) As String
        Dim fps, crf, audio, preset, pix As String
        'presets: ultrafast,superfast,veryfast,faster,fast,medium,slow,slower,veryslow
        crf = "30" 'My.Settings.ffmpegCRF
        fps = "20" 'My.Settings.ffmpegOutFps
        If My.Settings.NoAudio Or Not DoAudio Then
            audio = ""
        Else
            audio = " -map 0:a -c:a aac -ac 2 -ar 48000 -b:a 128k"
        End If

        preset = "ultrafast" 'My.Settings.ffmpegPreset
        FFMPeg_MakeQuickOutputStr = " -c:v libx264 -tune fastdecode -crf " + crf + audio + " -r " + fps + " -preset " + preset
        'FFMPeg_MakeQuickOutputStr = " -c:v libx264 -crf " + crf + audio + " -r " + fps + " -preset " + preset
        pix = " -pix_fmt yuvj420p"
        If length <> "" And length <> "0" Then
            FFMPeg_MakeQuickOutputStr += " -t " + length
        End If

        FFMPeg_MakeQuickOutputStr += pix + " -y " + """" + outputfile + """"
    End Function

    Function FFMPeg_MakeJoinStr(VideoList As String, joinfile As String, crf As String, fps As String, preset As String, Optional length As String = "") As String
        Dim filepath, filecontent, tmp As String
        Dim i As Integer
        ' Make joinfile
        filepath = AppFolder + joinfile
        tmp = VideoList
        If VideoList <> "" Then
            i = VideoList.IndexOf(",")
            If i > 0 Then
                filecontent = "file '" + VideoList.Substring(0, i) + "'"
                tmp = VideoList.Substring(i + 1)
                Do

                    i = tmp.IndexOf(",")
                    If i >= 0 Then
                        filecontent += vbCrLf + "file '"
                        filecontent += tmp.Substring(0, i) + "'"
                        tmp = tmp.Substring(i + 1)

                    End If
                Loop Until i < 0
                filecontent += vbCrLf + "file '"
                filecontent += tmp + "'"
            Else
                'only one file
                filecontent = "file '" + VideoList + "'"
            End If

            filecontent = filecontent.Replace("""", "")

            My.Computer.FileSystem.WriteAllText(JoinListFile, filecontent, False, New UTF8Encoding(False))


        End If
        FFMPeg_MakeJoinStr = My.Settings.ffmpegJoin + FFMPeg_MakeOutputStr(joinfile, length, True, False)
    End Function
    Function SecToTimeStr(Seconds As Single) As String
        Dim spanTM As New TimeSpan(TimeSpan.TicksPerSecond * Seconds)
        Dim TimeStr As String = spanTM.Hours.ToString("0") & ":" & spanTM.Minutes.ToString("00") & ":" & spanTM.Seconds.ToString("00")
        SecToTimeStr = TimeStr
    End Function
    Function SecToTimeStr(Seconds As String) As String
        If Seconds.IndexOf(":") < 0 Then ' Hvis det er en tidsstring med : så skal der ikke beregnes

            Dim spanTM As New TimeSpan(TimeSpan.TicksPerSecond * Seconds)
            Dim TimeStr As String = spanTM.Hours.ToString("0") & ":" & spanTM.Minutes.ToString("00") & ":" & spanTM.Seconds.ToString("00")
            SecToTimeStr = TimeStr
        Else
            SecToTimeStr = Seconds
        End If
    End Function

    Function TimeStrToSec(TimeStr As String) As Integer
        Try
            Dim timeArray() As String = TimeStr.Split(":"c)
            Dim hours As Integer = 0
            Dim minutes As Integer = 0
            Dim seconds As Integer = 0

            If timeArray.Length = 1 Then
                seconds = CInt(timeArray(0))
            ElseIf timeArray.Length = 2 Then
                minutes = CInt(timeArray(0))
                seconds = CInt(timeArray(1))

            ElseIf timeArray.Length = 3 Then
                hours = CInt(timeArray(0))
                minutes = CInt(timeArray(1))
                seconds = CInt(timeArray(2))
            Else
                ' Handle invalid time string format '
                ' You can throw an exception or return a default value '
            End If

            Return (hours * 3600) + (minutes * 60) + seconds

        Catch ex As Exception
            Return -1
        End Try

    End Function
    Function GetVideoFileDuration(ByVal MovieFullPath As String) As String
        MovieFullPath = MovieFullPath.Replace("""", "")
        If File.Exists(MovieFullPath) Then
            Dim objShell As Object = CreateObject("Shell.Application")
            Dim objFolder As Object =
                objShell.Namespace(Path.GetDirectoryName(MovieFullPath))
            For Each strFileName In objFolder.Items
                If strFileName.Name = Path.GetFileName(MovieFullPath) Then
                    Dim objFolderItem As Object = strFileName
                    Dim PropList As Object =
                        objFolder.GetDetailsOf(objFolderItem, 27)
                    Return PropList
                End If
            Next
        End If
        Return ""
    End Function

    Public Shared Sub PS_multi_FF_Script(
        inputFolder As String,
        outputFolder As String,
        fileExtension As String,
        ffmpegArgs As String
    )
        ' Create a unique batch file name
        Dim batchFileName As String = Path.Combine(outputFolder, "ExecutePowerShellScript.cmd")

        ' Create the batch file content
        Dim batchFileContent As String
        batchFileContent = "@echo off"+vbCrLf

        batchFileContent += $"powershell.exe -ExecutionPolicy Bypass -File ""Ffmpegparallel.ps1"" -inputFolder ""{inputFolder}"" -outputFolder ""{outputFolder}"" -fileExtension ""{fileExtension}"" -ffmpegArgs ""{ffmpegArgs}"""
        batchFileContent += vbCrLf + "pause"

        Try
            ' Write the batch file content to the output folder
            File.WriteAllText(batchFileName, batchFileContent)

            ' Execute the generated batch file
            Dim psi As New ProcessStartInfo()
            psi.FileName = batchFileName
            psi.UseShellExecute = True ' UseShellExecute set to True to display the window
            psi.CreateNoWindow = False ' CreateNoWindow set to False to show the window

            Dim process As New Process()
            process.StartInfo = psi
            process.Start()
            process.WaitForExit()
        Catch ex As Exception
            ' Handle any exceptions here
            MsgBox($"Error: {ex.Message}")
        End Try
    End Sub
End Class

