// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.FaceMesh
{
    public class MakeUserInfo : ImageSourceSolution<FaceMeshGraph>
    {
        [SerializeField] private DetectionListAnnotationController _faceDetectionsAnnotationController;
        [SerializeField] private MultiFaceLandmarkListAnnotationController _multiFaceLandmarksAnnotationController;
        [SerializeField] private NormalizedRectListAnnotationController _faceRectsFromLandmarksAnnotationController;
        [SerializeField] private NormalizedRectListAnnotationController _faceRectsFromDetectionsAnnotationController;




        public int maxNumFaces
        {
            get => graphRunner.maxNumFaces;
            set => graphRunner.maxNumFaces = value;
        }

        public bool refineLandmarks
        {
            get => graphRunner.refineLandmarks;
            set => graphRunner.refineLandmarks = value;
        }

        public float minDetectionConfidence
        {
            get => graphRunner.minDetectionConfidence;
            set => graphRunner.minDetectionConfidence = value;
        }

        public float minTrackingConfidence
        {
            get => graphRunner.minTrackingConfidence;
            set => graphRunner.minTrackingConfidence = value;
        }

        Mediapipe.Unity.ImageSource imageSource; 
        protected override void OnStartRun()
        {
            
            if (!runningMode.IsSynchronous())
            {
                graphRunner.OnFaceDetectionsOutput += OnFaceDetectionsOutput;
                graphRunner.OnMultiFaceLandmarksOutput += OnMultiFaceLandmarksOutput;
                graphRunner.OnFaceRectsFromLandmarksOutput += OnFaceRectsFromLandmarksOutput;
                graphRunner.OnFaceRectsFromDetectionsOutput += OnFaceRectsFromDetectionsOutput;
            }
            imageSource = ImageSourceProvider.ImageSource; 
            SetupAnnotationController(_faceDetectionsAnnotationController, imageSource);
            SetupAnnotationController(_faceRectsFromLandmarksAnnotationController, imageSource);
            SetupAnnotationController(_multiFaceLandmarksAnnotationController, imageSource);
            SetupAnnotationController(_faceRectsFromDetectionsAnnotationController, imageSource);
            
        }

        protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
        {
            graphRunner.AddTextureFrameToInputStream(textureFrame);
        }

        protected override IEnumerator WaitForNextValue()
        {
            List<Detection> faceDetections = null;
            List<NormalizedLandmarkList> multiFaceLandmarks = null;
            List<NormalizedRect> faceRectsFromLandmarks = null;
            List<NormalizedRect> faceRectsFromDetections = null;

            if (runningMode == RunningMode.Sync)
            {
                var _ = graphRunner.TryGetNext(out faceDetections, out multiFaceLandmarks, out faceRectsFromLandmarks, out faceRectsFromDetections, true);
            }
            else if (runningMode == RunningMode.NonBlockingSync)
            {
                yield return new WaitUntil(() => graphRunner.TryGetNext(out faceDetections, out multiFaceLandmarks, out faceRectsFromLandmarks, out faceRectsFromDetections, false));
            }

            _faceDetectionsAnnotationController.DrawNow(faceDetections);
            _multiFaceLandmarksAnnotationController.DrawNow(multiFaceLandmarks);
            _faceRectsFromLandmarksAnnotationController.DrawNow(faceRectsFromLandmarks);
            _faceRectsFromDetectionsAnnotationController.DrawNow(faceRectsFromDetections);
        }

        private void OnFaceDetectionsOutput(object stream, OutputEventArgs<List<Detection>> eventArgs)
        {
            _faceDetectionsAnnotationController.DrawLater(eventArgs.value);
        }

        private void OnMultiFaceLandmarksOutput(object stream, OutputEventArgs<List<NormalizedLandmarkList>> eventArgs)
        {
            _multiFaceLandmarksAnnotationController.DrawLater(eventArgs.value);

            if (eventArgs.value != null)
            {
                if(count % 180 == 0)
                {
                    Debug.Log("ready");
                    /* get user face info */
                    // ## 얼굴형
                    // 얼굴 너비 구하기
                    Vector3 face_127 = new Vector3(eventArgs.value[0].Landmark[127].X, eventArgs.value[0].Landmark[127].Y, eventArgs.value[0].Landmark[127].Z);
                    Vector3 face_356 = new Vector3(eventArgs.value[0].Landmark[356].X, eventArgs.value[0].Landmark[356].Y, eventArgs.value[0].Landmark[356].Z);

                    double face_w = GetInstance(face_127, face_356);
                    GameManager.Instance.userData.FaceWidth = face_w;

                    // 얼굴 높이 구하기
                    Vector3 face_10 = new Vector3(eventArgs.value[0].Landmark[10].X, eventArgs.value[0].Landmark[10].Y, eventArgs.value[0].Landmark[10].Z);
                    Vector3 face_152 = new Vector3(eventArgs.value[0].Landmark[152].X, eventArgs.value[0].Landmark[152].Y, eventArgs.value[0].Landmark[152].Z);

                    double face_h = GetInstance(face_10, face_152);
                    GameManager.Instance.userData.FaceHeight = face_h;

                    // 턱 너비 구하기
                    Vector3 chin_172 = new Vector3(eventArgs.value[0].Landmark[172].X, eventArgs.value[0].Landmark[172].Y, eventArgs.value[0].Landmark[172].Z);
                    Vector3 chin_397 = new Vector3(eventArgs.value[0].Landmark[397].X, eventArgs.value[0].Landmark[397].Y, eventArgs.value[0].Landmark[397].Z);

                    double chin_w = GetInstance(chin_172, chin_397);
                    GameManager.Instance.userData.ChinWidth = chin_w;


                    // ## 코
                    // 코 너비 구하기
                    Vector3 nose_220 = new Vector3(eventArgs.value[0].Landmark[220].X, eventArgs.value[0].Landmark[220].Y, eventArgs.value[0].Landmark[220].Z);
                    Vector3 nose_440 = new Vector3(eventArgs.value[0].Landmark[440].X, eventArgs.value[0].Landmark[440].Y, eventArgs.value[0].Landmark[440].Z);

                    double nose_w = GetInstance(nose_220, nose_440);
                    GameManager.Instance.userData.NoseWidth = nose_w;

                    // 코 세로 길이 구하기
                    Vector3 nose_6 = new Vector3(eventArgs.value[0].Landmark[6].X, eventArgs.value[0].Landmark[6].Y, eventArgs.value[0].Landmark[6].Z);
                    Vector3 nose_19 = new Vector3(eventArgs.value[0].Landmark[19].X, eventArgs.value[0].Landmark[19].Y, eventArgs.value[0].Landmark[19].Z);

                    double nose_h = GetInstance(nose_6, nose_19);
                    GameManager.Instance.userData.NoseHeight = nose_h;


                    // ## 입
                    // 입 넓이 구하기
                    Vector3 mouth_61 = new Vector3(eventArgs.value[0].Landmark[61].X, eventArgs.value[0].Landmark[61].Y, eventArgs.value[0].Landmark[61].Z);
                    Vector3 mouth_291 = new Vector3(eventArgs.value[0].Landmark[291].X, eventArgs.value[0].Landmark[291].Y, eventArgs.value[0].Landmark[291].Z);

                    double mouth_w = GetInstance(mouth_61, mouth_291);
                    GameManager.Instance.userData.MouthWidth = mouth_w;

                    // 윗입술 높이 구하기
                    Vector3 lip_0 = new Vector3(eventArgs.value[0].Landmark[0].X, eventArgs.value[0].Landmark[0].Y, eventArgs.value[0].Landmark[0].Z);
                    Vector3 lip_13 = new Vector3(eventArgs.value[0].Landmark[13].X, eventArgs.value[0].Landmark[13].Y, eventArgs.value[0].Landmark[13].Z);

                    double lip_h1 = GetInstance(lip_0, lip_13);
                    GameManager.Instance.userData.UpperLipHeight = lip_h1;

                    // 아랫입술 높이 구하기
                    Vector3 lip_14 = new Vector3(eventArgs.value[0].Landmark[14].X, eventArgs.value[0].Landmark[14].Y, eventArgs.value[0].Landmark[14].Z);
                    Vector3 lip_17 = new Vector3(eventArgs.value[0].Landmark[17].X, eventArgs.value[0].Landmark[17].Y, eventArgs.value[0].Landmark[17].Z);

                    double lip_h2 = GetInstance(lip_14, lip_17);
                    GameManager.Instance.userData.LowerLipHeight = lip_h2;


                    // ## 눈
                    // 미간 거리 구하기
                    Vector3 eye_133 = new Vector3(eventArgs.value[0].Landmark[133].X, eventArgs.value[0].Landmark[133].Y, eventArgs.value[0].Landmark[133].Z);
                    Vector3 eye_362 = new Vector3(eventArgs.value[0].Landmark[362].X, eventArgs.value[0].Landmark[362].Y, eventArgs.value[0].Landmark[362].Z);

                    double eye_d = GetInstance(eye_133, eye_362);
                    GameManager.Instance.userData.EyeDistance = eye_d;
                }
                else
                {
                    count++;
                }
                
                
            }
        }
        int count=1;
        private void Update()
        {
        }

        float GetAngle(Vector2 start, Vector2 end)
        {
            Vector2 v2 = end - start;
            return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
        }

        public GameObject ob;

        private double GetInstance(Vector3 vec_1, Vector3 vec_2)
        {
            Vector3 vec = new Vector3();

            float f = (vec_2.x - vec_1.x) * (vec_2.x - vec_1.x) + (vec_2.y - vec_1.y) * (vec_2.y - vec_1.y);

            double d = Convert.ToDouble(f);
            return Math.Sqrt(d);
        }

        private void OnFaceRectsFromLandmarksOutput(object stream, OutputEventArgs<List<NormalizedRect>> eventArgs)
        {
            _faceRectsFromLandmarksAnnotationController.DrawLater(eventArgs.value);
        }

        private void OnFaceRectsFromDetectionsOutput(object stream, OutputEventArgs<List<NormalizedRect>> eventArgs)
        {
            _faceRectsFromDetectionsAnnotationController.DrawLater(eventArgs.value);
        }
    }
}