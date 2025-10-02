using System;
using System.Collections.Generic;
using System.Text;
using KAsyncEngineLib;

public class EventHandler : KAEventHandler
{
	virtual public void OnLoadScene(eKResult Result, string SceneName) { }
	virtual public void OnLoadSceneForce(eKResult Result, string SceneName) { }
	virtual public void OnLogMessage(string LogMessage) { }
	virtual public void OnMessageNo(uint MessageNo) { }
	virtual public void OnConnect(int ErrorCode) { }	
	virtual public void OnClose(int ErrorCode) { }
	virtual public void OnBeginTransaction(eKResult Result) { }
	virtual public void OnEndTransaction(eKResult Result) { }	
	virtual public void OnHeartBeat(eKResult Result) { }
	virtual public void OnUnloadAll(eKResult Result) { }
	virtual public void OnSetTrialPlayoutMode(eKResult Result) { }
	virtual public void OnCheckVersion(eKResult Result, string ServerVersion, string SDKVersion) { }
	virtual public void OnSetAudioOutput(eKResult Result) { }		
	virtual public void OnScenePrepare(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnScenePrepareEx(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnPlay(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnPlayOut(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnStop(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnStopAll(eKResult Result) { }
	virtual public void OnPause(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnResume(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnQueryIsOnAir(eKResult Result, int OutputChannelIndex, int LayerNo, int bOnAir) { }
	virtual public void OnTrigger(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnScenePlayingStarted(string SceneName, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnScenePlayed(string SceneName, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnSceneAnimationPlayed(string SceneName, int OutputChannelIndex, int LayerNo, string AnimiationName) { }
	virtual public void OnScenePaused(string SceneName, int OutputChannelIndex, int LayerNo, int LastPause) { }
	virtual public void OnSceneSaved(eKResult Result, string FileName) { }
	virtual public void OnTriggerObject(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnResumeBackground(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnSaveMixedPreviewImage(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnPlayDirect(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnCutIn(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnCutOut(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnClearNextPreview(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnPlayRange(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnQueryPlaybackRangeCount(eKResult Result, string SceneName, int PlaybackRangeCount) { }
	virtual public void OnQueryPlaybackRange(eKResult Result, string SceneName, int PlaybackRangeNo, int Start, int End) { }
	virtual public void OnQueryOutputChannelIndex(eKResult Result, string SceneName, int OutputChannelIndex) { }
	virtual public void OnPlayInNextPreview(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnPlayOutNextPreview(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnSetBackgroundFill(eKResult Result, string SceneName) { }
	virtual public void OnSetBackgroundTexture(eKResult Result, string SceneName) { }
	virtual public void OnSetBackgroundVideo(eKResult Result, string SceneName) { }
	virtual public void OnSetBackgroundLiveIn(eKResult Result, string SceneName) { }
	virtual public void OnUseBackground(eKResult Result, string SceneName) { }
	virtual public void OnSetBackgroundVideoPlayInfo(eKResult Result, string SceneName) { }
	virtual public void OnQueryBackgroundVideoPlayInfo(eKResult Result, string SceneName, ref sKVideoPlayInfo pVideoPlayInfo) { }
	virtual public void OnSetSceneEffectType(eKResult Result, string SceneName) { }	
	virtual public void OnSaveSceneImage(eKResult Result, string SceneName) { }
	virtual public void OnSaveScene(eKResult Result, string SceneName) { }
	virtual public void OnUnloadScene(eKResult Result, string SceneName) { }
	virtual public void OnReloadScene(eKResult Result, string SceneName) { }
	virtual public void OnUpdateTextures(eKResult Result, string SceneName) { }
	virtual public void OnSetSceneAudioFile(eKResult Result, string SceneName) { }
	virtual public void OnEnableSceneAudio(eKResult Result, string SceneName) { }
	virtual public void OnSetSceneDuration(eKResult Result, string SceneName) { }
	virtual public void OnSetBackgroundPauseType(eKResult Result, string SceneName) { }
	virtual public void OnSetBackgroundChangeType(eKResult Result, string SceneName) { }
	virtual public void OnSetBackgroundPauseAtZeroFrameAsStandBy(eKResult Result, string SceneName) { }
	virtual public void OnResetDuration(eKResult Result, string SceneName) { }
	virtual public void OnSetDuration(eKResult Result, string SceneName) { }
	virtual public void OnAddObject(eKResult Result, string SceneName) { }
	virtual public void OnAddCloneObject(eKResult Result, string SceneName) { }
	virtual public void OnUpdateThumbnail(eKResult Result, string SceneName) { }
	virtual public void OnExportVideo(eKResult Result, string SceneName) { }
	virtual public void OnStopVideoExporting(eKResult Result) { }
	virtual public void OnQueryVideoExportingProgress(eKResult Result, string TargetName, int CurrentFrame, int TotalFrame) { }
	virtual public void OnFinishedVideoExporting(eKResult Result, string FileName) { }
	virtual public void OnAddPause(eKResult Result, string SceneName) { }
	virtual public void OnDeletePause(eKResult Result, string SceneName) { }
	virtual public void OnSetPause(eKResult Result, string SceneName) { }
	virtual public void OnSetPauseWithIndex(eKResult Result, string SceneName) { }
	virtual public void OnDeletePauseWithIndex(eKResult Result, string SceneName) { }
	virtual public void OnQueryPauseCount(eKResult Result, string SceneName, int PauseCount) { }
	virtual public void OnQueryObjectInfos(eKResult Result, string SceneName, KAObjectInfos pObjectInfos) { }
	virtual public void OnQueryAnimationNames(eKResult Result, string SceneName, KAStrings pAnimationNames) { }
	virtual public void OnQueryAnimationCount(eKResult Result, string SceneName, int AnimationCount) { }
	virtual public void OnQueryObjectInfosByScreenPoint(eKResult Result, KAObjectInfos pObjectInfos) { }
	virtual public void OnQuerySceneEffectType(eKResult Result, string SceneName, int bInEffect, eKEffectType EffectType, int Duration) { }
	virtual public void OnQueryDuration(eKResult Result, string SceneName, string AnimationName, int Duration) { }
	virtual public void OnQueryContentsOfTextObjects(eKResult Result, string SceneName, KAStrings pTexts) { }
	virtual public void OnSetStyleColor(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetStyleTexture(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetFaceTextColor(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetEdgeTextColor(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetShadowTextColor(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetVisible(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetValue(eKResult Result, string SceneName, string ObjectName) { }	
	virtual public void OnAddText(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnStoreTextStyle(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetTextStyle(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnEditText(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetFont(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetTextRange(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnResetTextRange(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnQueryObjectType(eKResult Result, string SceneName, string ObjectName, eKObjectType ObjectType) { }
	virtual public void OnSetChartCSVFile(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetChartCellData(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnQueryChartDataTable(eKResult Result, string SceneName, string ObjectName, KAChartDataTable Table) { }
	virtual public void OnQuerySize(eKResult Result, string SceneName, string ObjectName, float Width, float Height) { }
	virtual public void OnSetSize(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetCounterNumberKey(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetPositionKey(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetRotationKey(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetScaleKey(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetCylinderAngleKey(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetSphereAngleKey(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetCircleAngleKey(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetCropKey(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetCountDown(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetPosition(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetRotation(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetScale(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnAddPathPoint(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnClearPathPoints(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnAddPathShapePoint(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnClearPathShapePoints(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnQueryScrollRemainingDistance(eKResult Result, string SceneName, string ObjectName, int ScrollRemainingDistance) { }
	virtual public void OnQueryScrollChildRemainingDistance(eKResult Result, string SceneName, string ObjectName, string ChildName, int ScrollRemainingDistance) { }
	virtual public void OnAddScrollObject(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnAdjustScrollSpeed(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetScrollSpeed(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetVariableName(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetLoftPositionKey(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetChangeOut(eKResult Result, string SceneName) { }
	virtual public void OnModifyPathPoint(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnInitScrollObject(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetCounterInfo(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetCounterNumber(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetCounterRange(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetCounterRemainingTime(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetCounterElapsedTime(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSaveObjectImage(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetPositionOfPathAnimation(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetPositionKeyOfPathAnimation(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetStartFrame(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetObjectEffectType(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetObjectOutEffectDelay(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetColor(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetColorKey(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetEmissiveColor(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetEmissiveColorKey(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetTransparencyOpacity(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetTransparencyOpacityKey(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetExposure(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetExposureKey(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetMaterialTextureType(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetMaterialTextureFile(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetMaterialTextureOffset(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetMaterialTextureOffsetKey(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetMaterialTextureTiling(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetMaterialTextureTilingKey(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetMaterialTextureRotation(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetMaterialTextureRotationKey(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetMaterialTextureOpacity(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetMaterialTextureOpacityKey(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnQueryGroupType(eKResult Result, string SceneName, string ObjectName, eKGroupType GroupType) { }
	virtual public void OnQueryImageType(eKResult Result, string SceneName, string ObjectName, eKImageType ImageType) { }
	virtual public void OnSetVideoPlayInfo(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnQueryVideoPlayInfo(eKResult Result, string SceneName, string ObjectName, ref sKVideoPlayInfo pVideoPlayInfo) { }
	virtual public void OnQueryIs3D(eKResult Result, string SceneName, string ObjectName, int b3D) { }
	virtual public void OnQueryPosition(eKResult Result, string SceneName, string ObjectName, float X, float Y, float Z) { }
	virtual public void OnSetImageType(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetMemo(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnQueryMemo(eKResult Result, string SceneName, string ObjectName, string Memo) { }
	virtual public void OnQueryFont(eKResult Result, string SceneName, string ObjectName, ref sKFont Param) { }
	virtual public void OnSetImageOriginalSize(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnApplyChangeEffectLibrary(eKResult Result, string SceneName) { }
	virtual public void OnApplyObjectLibrary(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnApplyTextureEffectLibrary(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetTableValue(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetTableColor(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnQueryTableValue(eKResult Result, string SceneName, string ObjectName, int Row, int Column, string Value) { }
	virtual public void OnQueryTableValues(eKResult Result, string SceneName, string ObjectName, KATableValues pValues) { }
	virtual public void OnSetPathShapeOutlineThickness(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnEnablePathShapeOutline(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetPlaybackCamera(eKResult Result, string SceneName) { }
	virtual public void OnSetMaterialTextureVideoPlayInfo(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnQueryMaterialTextureVideoPlayInfo(eKResult Result, string SceneName, string ObjectName, ref sKVideoPlayInfo VideoPlayInfo) { }
	virtual public void OnQueryVideoFormat(eKResult Result, ref sKVideoFormat VideoFormat) { }
	virtual public void OnQueryLiveStreamingStatus(eKResult Result, string StreamingURI, eKLiveStreamingStatus Status) { }
	virtual public void OnPreloadLiveStreaming(eKResult Result, string StreamingURI) { }
	virtual public void OnReleaseLiveStreaming(eKResult Result, string StreamingURI) { }
	virtual public void OnUpdateImageResource(eKResult Result) { }
	virtual public void OnQueryLayerCount(eKResult Result, int LayerCount) { }
	virtual public void OnSetLayerViewportRate(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnSetLayerViewportRateEx(eKResult Result, int OutputChannelIndex, int LayerNo) { }
	virtual public void OnSetFitting(eKResult Result, string SceneName) { }
	virtual public void OnSetFittingOffset(eKResult Result, string SceneName) { }
	virtual public void OnSetFittingScale(eKResult Result, string SceneName) { }
	virtual public void OnSetLightColor(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnEnableLight(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetDirectionalLight(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetPointLight(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetSpotLight(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetInfinitePointLight(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetMaterialTextureLiveStreamingURI(eKResult Result, string SceneName, string ObjectName) { }
	virtual public void OnSetBackgroundLiveStreamingURI(eKResult Result, string SceneName) { }
	virtual public void OnLoadProject(eKResult Result, string FilePath, string AliasName) { }
	virtual public void OnNewProject(eKResult Result, string AliasName) { }
	virtual public void OnUnloadAllProject(eKResult Result) { }
	virtual public void OnSaveProject(eKResult Result, string AliasName) { }
	virtual public void OnQuerySceneItemCount(eKResult Result, string AliasName, int SceneItemCount) { }
	virtual public void OnQuerySceneItemInfos(eKResult Result, string AliasName, KASceneItemInfos SceneItemInfos) { }
	virtual public void OnAddSceneItem(eKResult Result, string AliasName, int Index) { }
	virtual public void OnInsertSceneItem(eKResult Result, string AliasName) { }
	virtual public void OnDeleteSceneItem(eKResult Result, string AliasNAme) { }
	virtual public void OnQueryProjectFormat(eKResult Result, ref sKVideoFormat ProjectFormat) { }
	virtual public void OnSetTimecode(eKResult Result, string AliasName) { }
	virtual public void OnSetTimecodeInOut(eKResult Result, string AliasName) { }
	virtual public void OnSetTimecodeTrack(eKResult Result, string AliasName) { }
	virtual public void OnSetTimecodeInOutType(eKResult Result, string AliasName) { }
	virtual public void OnDeleteTimecode(eKResult Result, string AliasName) { }
	virtual public void OnQueryTimecode(eKResult Result, int TrackNo, int In, int Out, int bOnTrack) { }
	virtual public void OnUnloadProject(eKResult Result, string AliasName) { }
	virtual public void OnEnableSyncWithSceneEffect(eKResult Result, string AliasName) { }
	virtual public void OnExportProjectVideo(eKResult Result, string AliasName) { }
	virtual public void OnExportSceneImage(eKResult Result, string SceneName) { }
	virtual public void OnStartVideoCapture(eKResult Result) { }
	virtual public void OnStopVideoCapture(eKResult Result) { }
	virtual public void OnCaptureImage(eKResult Result) { }

}

