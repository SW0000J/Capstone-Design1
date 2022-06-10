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
    public class FaceMeshSolution : ImageSourceSolution<FaceMeshGraph>
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

        protected override void OnStartRun()
        {
            if (!runningMode.IsSynchronous())
            {
                graphRunner.OnFaceDetectionsOutput += OnFaceDetectionsOutput;
                graphRunner.OnMultiFaceLandmarksOutput += OnMultiFaceLandmarksOutput;
                graphRunner.OnFaceRectsFromLandmarksOutput += OnFaceRectsFromLandmarksOutput;
                graphRunner.OnFaceRectsFromDetectionsOutput += OnFaceRectsFromDetectionsOutput;
            }

            var imageSource = ImageSourceProvider.ImageSource;
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
                Vector3 left_33 = new Vector3(eventArgs.value[0].Landmark[33].X, eventArgs.value[0].Landmark[33].Y, eventArgs.value[0].Landmark[33].Z);
                Vector3 left_133 = new Vector3(eventArgs.value[0].Landmark[133].X, eventArgs.value[0].Landmark[133].Y, eventArgs.value[0].Landmark[133].Z);

                Vector3 left_160 = new Vector3(eventArgs.value[0].Landmark[160].X, eventArgs.value[0].Landmark[160].Y, eventArgs.value[0].Landmark[160].Z);
                Vector3 left_144 = new Vector3(eventArgs.value[0].Landmark[144].X, eventArgs.value[0].Landmark[144].Y, eventArgs.value[0].Landmark[144].Z);

                Vector3 left_158 = new Vector3(eventArgs.value[0].Landmark[158].X, eventArgs.value[0].Landmark[158].Y, eventArgs.value[0].Landmark[158].Z);
                Vector3 left_153 = new Vector3(eventArgs.value[0].Landmark[153].X, eventArgs.value[0].Landmark[153].Y, eventArgs.value[0].Landmark[153].Z);

                double A = GetInstance(left_160, left_144);
                double B = GetInstance(left_158, left_153);
                double C = GetInstance(left_33, left_133);

                double ear = (A + B) / (2 * C);

                Vector3 right_263 = new Vector3(eventArgs.value[0].Landmark[263].X, eventArgs.value[0].Landmark[263].Y, eventArgs.value[0].Landmark[263].Z);
                Vector3 right_362 = new Vector3(eventArgs.value[0].Landmark[362].X, eventArgs.value[0].Landmark[362].Y, eventArgs.value[0].Landmark[362].Z);

                Vector3 right_387 = new Vector3(eventArgs.value[0].Landmark[387].X, eventArgs.value[0].Landmark[387].Y, eventArgs.value[0].Landmark[387].Z);
                Vector3 right_373 = new Vector3(eventArgs.value[0].Landmark[373].X, eventArgs.value[0].Landmark[373].Y, eventArgs.value[0].Landmark[373].Z);

                Vector3 right_385 = new Vector3(eventArgs.value[0].Landmark[385].X, eventArgs.value[0].Landmark[385].Y, eventArgs.value[0].Landmark[385].Z);
                Vector3 right_380 = new Vector3(eventArgs.value[0].Landmark[380].X, eventArgs.value[0].Landmark[380].Y, eventArgs.value[0].Landmark[380].Z);

                double right_blink_A = GetInstance(right_387, right_373);
                double right_blink_B = GetInstance(right_385, right_380);
                double right_blink_C = GetInstance(right_263, right_362);

                double right_ear = (right_blink_A + right_blink_B) / (2 * right_blink_C);


                //bool leftEyesBlink = false;


                if (GameManager.Instance == null)
                {
                    Debug.Log(111);
                }
                GameManager.Instance.left_eye_blink = ear;
                GameManager.Instance.right_eye_blink = right_ear;
                if (ear < 0.25f)
                {

                    //leftEyesBlink = true;
                    //Debug.Log("눈 감음!!");
                }

                Vector3 left_82 = new Vector3(eventArgs.value[0].Landmark[82].X, eventArgs.value[0].Landmark[82].Y, eventArgs.value[0].Landmark[82].Z);
                Vector3 left_87 = new Vector3(eventArgs.value[0].Landmark[87].X, eventArgs.value[0].Landmark[87].Y, eventArgs.value[0].Landmark[87].Z);

                Vector3 left_312 = new Vector3(eventArgs.value[0].Landmark[312].X, eventArgs.value[0].Landmark[312].Y, eventArgs.value[0].Landmark[312].Z);
                Vector3 left_317 = new Vector3(eventArgs.value[0].Landmark[317].X, eventArgs.value[0].Landmark[317].Y, eventArgs.value[0].Landmark[317].Z);

                Vector3 left_78 = new Vector3(eventArgs.value[0].Landmark[78].X, eventArgs.value[0].Landmark[78].Y, eventArgs.value[0].Landmark[78].Z);
                Vector3 left_30 = new Vector3(eventArgs.value[0].Landmark[30].X, eventArgs.value[0].Landmark[30].Y, eventArgs.value[0].Landmark[30].Z);

                double Mouse_A = GetInstance(left_82, left_87);
                double Mouse_B = GetInstance(left_312, left_317);
                double Mouse_C = GetInstance(left_78, left_30);

                double Mouse = (Mouse_A + Mouse_B) / (2 * Mouse_C);

                GameManager.Instance.mouth_open = Mouse;

                bool mouse_Flag = false;
                if (Mouse > 0.2f)
                {
                    mouse_Flag = true;
                    //Debug.Log("입 열림");
                }




                // 머리 회전 hong.ver



                //double[,] face_2d = new double[,] { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };
                //double[,] face_3d = new double[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };

                /*
              
                float img_w = 640;
                float img_h = 480;

                // idx 1
                (float, float) nose_2d = (eventArgs.value[0].Landmark[1].X * img_w, eventArgs.value[0].Landmark[1].Y * img_h); //img_w과 720은 웹캠해상도
                (float, float, float) nose_3d = (eventArgs.value[0].Landmark[1].X * img_w, eventArgs.value[0].Landmark[1].Y * img_h, eventArgs.value[0].Landmark[1].Z * 8000); //1280과 720은 웹캠해상도

                float idx1_x = Mathf.Round(eventArgs.value[0].Landmark[1].X * img_w);
                float idx1_y = Mathf.Round(eventArgs.value[0].Landmark[1].Y * img_h);

                // idx 33
                float idx33_x = Mathf.Round(eventArgs.value[0].Landmark[33].X * img_w);
                float idx33_y = Mathf.Round(eventArgs.value[0].Landmark[33].Y * img_h);

                // idx 263
                float idx263_x = Mathf.Round(eventArgs.value[0].Landmark[263].X * img_w);
                float idx263_y = Mathf.Round(eventArgs.value[0].Landmark[263].Y * img_h);

                // idx 61
                float idx61_x = Mathf.Round(eventArgs.value[0].Landmark[61].X * img_w);
                float idx61_y = Mathf.Round(eventArgs.value[0].Landmark[61].Y * img_h);

                // idx 291
                float idx291_x = Mathf.Round(eventArgs.value[0].Landmark[291].X * img_w);
                float idx291_y = Mathf.Round(eventArgs.value[0].Landmark[291].Y * img_h);

                // idx 199
                float idx199_x = Mathf.Round(eventArgs.value[0].Landmark[199].X * img_w);
                float idx199_y = Mathf.Round(eventArgs.value[0].Landmark[199].Y * img_h);

                var rvec = new double[] {0, 0, 0 };
                var tvec = new double[] { 0, 0, 0 };

                double focal_length = 1 * (double)img_w;

                double[,] cam_matrix = new double[,] { { focal_length, 0, (double)img_h / 2 }, { 0, focal_length, (double)img_w / 2 }, { 0, 0, 1 } };


                var dist_matrix = new double[] { 0, 0, 0, 0 };

                var objPts = new Point3f[]
                {
                    new Point3f(idx33_x,idx33_y,eventArgs.value[0].Landmark[33].Z),
                    new Point3f(idx263_x,idx263_y,eventArgs.value[0].Landmark[263].Z),
                    new Point3f(idx1_x,idx1_y,eventArgs.value[0].Landmark[1].Z),
                    new Point3f(idx61_x,idx61_y,eventArgs.value[0].Landmark[61].Z),
                    new Point3f(idx291_x,idx291_y,eventArgs.value[0].Landmark[291].Z),
                    new Point3f(idx199_x,idx199_y,eventArgs.value[0].Landmark[199].Z)
                };

                var imgPts = new Point2f[]
                {
                    new Point2f(idx33_x,idx33_y),
                    new Point2f(idx263_x,idx263_y),
                    new Point2f(idx1_x,idx1_y),
                    new Point2f(idx61_x,idx61_y),
                    new Point2f(idx291_x,idx291_y),
                    new Point2f(idx199_x,idx199_y)
                };

                //double[,] jacobian;

                //Point2f[] imgPts;



                //Cv2.ProjectPoints(objPts, rvec, tvec, cam_matrix, dist_matrix, out imgPts, out jacobian);

                Cv2.SolvePnP(objPts, imgPts, cam_matrix, dist_matrix, out rvec, out tvec);

                
                Cv2.Rodrigues(rvec, out var rmat, out var jac);

                Vec3d angles;   //retval
                angles = Cv2.RQDecomp3x3(rmat, out var matxR, out var mtxQ, out var qx, out var qy, out var qz);

            

                x = angles[0] * -360;
                y = angles[1] * 360;
                z = angles[2] * 360;




                if (count % 60 == 0)
                {
                    if (y < -10)
                    {
                        Debug.Log("Looking Right");
                    }
                    else if (y > 10)
                    {
                        Debug.Log("Looking Left");
                    }
                    else if (x < -10)
                    {
                        Debug.Log("Looking Up");
                    }
                    else if (x > 10)
                    {
                        Debug.Log("Looking Down");
                    }
                    else
                    {
                        Debug.Log("Forward");
                    }
                    count++;
                }
                else
                {
                    count++;
                }
                

                */





                /*
                face_2d[0, 0] = idx1_x;
                face_2d[0, 1] = idx1_y;
                face_2d[1, 0] = idx33_x;
                face_2d[1, 1] = idx33_y;
                face_2d[2, 0] = idx263_x;
                face_2d[2, 1] = idx263_y;
                face_2d[3, 0] = idx61_x;
                face_2d[3, 1] = idx61_y;
                face_2d[4, 0] = idx291_x;
                face_2d[4, 1] = idx291_y;
                face_2d[5, 0] = idx199_x;
                face_2d[5, 1] = idx199_y;

                for (int i = 0; i < 6; i++)
                {
                    face_3d[i, 0] = face_2d[i, 0];
                    face_3d[i, 1] = face_2d[i, 1];
                }
                face_3d[0, 2] = eventArgs.value[0].Landmark[1].Z;
                face_3d[1, 2] = eventArgs.value[0].Landmark[33].Z;
                face_3d[2, 2] = eventArgs.value[0].Landmark[263].Z;
                face_3d[3, 2] = eventArgs.value[0].Landmark[61].Z;
                face_3d[4, 2] = eventArgs.value[0].Landmark[291].Z;
                face_3d[5, 2] = eventArgs.value[0].Landmark[199].Z;


                var face_2d_n = np.array(face_2d, np.float64);   //일단 double로 근데 np.array(face_2d, np.float64로 면 되는데)
                var face_3d_n = np.array(face_3d, np.float64);

                //Debug.Log(face_2d_n);


                // Thre camera matrix
                double focal_length = 1 * (double)img_w;


                double[,] cam_matrix_param = new double[,] { { focal_length, 0, (double)img_h / 2},{ 0, focal_length, (double)img_w / 2},{ 0, 0, 1}, };
                var cam_matrix = np.array(cam_matrix_param); 

                var dist_matrix = np.zeros((4, 1), np.@double);

                InputArray inarray = InputArray.Create(face_2d);
                List<double> outarraylist = new List<double>();
                Cv2.ConvexHull(inarray, OutputArray.Create(outarraylist), false, true);
                Debug.Log(outarraylist);
                
                //Cv2.SolvePnP(face_3d_n, face_2d_n, cam_matrix, dist_matrix, out imgPts, out jacobian);

                */







                int sacle = 360;


                // 머리 회전
                vertextList = new Vector3(eventArgs.value[0].Landmark[5].X * sacle, eventArgs.value[0].Landmark[5].Y * sacle, eventArgs.value[0].Landmark[5].Z * sacle);
                ///


                Vector3 test_left_1 = new Vector3(eventArgs.value[0].Landmark[5].X, eventArgs.value[0].Landmark[5].Y, eventArgs.value[0].Landmark[5].Z);
                Vector3 test_left_2 = new Vector3(eventArgs.value[0].Landmark[67].X, eventArgs.value[0].Landmark[67].Y, eventArgs.value[0].Landmark[67].Z);

                Vector3 test_right_1 = new Vector3(eventArgs.value[0].Landmark[5].X, eventArgs.value[0].Landmark[5].Y, eventArgs.value[0].Landmark[5].Z);
                Vector3 test_right_2 = new Vector3(eventArgs.value[0].Landmark[297].X, eventArgs.value[0].Landmark[297].Y, eventArgs.value[0].Landmark[297].Z);


                int idx_1 = 55;
                int idx_2 = 127;

                Vector3 test_up_1 = new Vector3(eventArgs.value[0].Landmark[idx_1].X, eventArgs.value[0].Landmark[idx_1].Y, eventArgs.value[0].Landmark[idx_1].Z);
                Vector3 test_up_2 = new Vector3(eventArgs.value[0].Landmark[idx_2].X, eventArgs.value[0].Landmark[idx_2].Y, eventArgs.value[0].Landmark[idx_2].Z);

                int idx_3 = 276;
                int idx_4 = 356;

                Vector3 test_down_1 = new Vector3(eventArgs.value[0].Landmark[idx_3].X, eventArgs.value[0].Landmark[idx_3].Y, eventArgs.value[0].Landmark[idx_3].Z);
                Vector3 test_down_2 = new Vector3(eventArgs.value[0].Landmark[idx_4].X, eventArgs.value[0].Landmark[idx_4].Y, eventArgs.value[0].Landmark[idx_4].Z);





                /// 이하 머리 회전

                test_left = GetAngle(test_left_1, test_left_2);
                test_right = GetAngle(test_right_1, test_right_2);


                test_up = GetAngle(test_up_1, test_up_2);
                test_down = GetAngle(test_down_1, test_down_2);



                y = (float)(test_left + test_right) / 2 - 85;
                z = vertextList.y;

                /////

            }
        }
        private Vector3 vertextList = new Vector3();
        double test_left = 0;
        double test_right = 0;

        double test_up = 0;
        double test_down = 0;

        float x = 0;
        float y = 0;
        float z = 0;

        private void Update()
        {
           
           ob.transform.rotation = Quaternion.Euler(0, y * 2, z);// 머리 회전 오브젝트
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