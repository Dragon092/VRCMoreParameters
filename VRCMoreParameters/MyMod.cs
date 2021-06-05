using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using UnityEngine;
using VRC;
using VRC.Core;
using ParamLib;

namespace VRCMoreParameters
{
    public class MyMod : MelonMod
    {
        public VRCPlayer controller = VRCPlayer.field_Internal_Static_VRCPlayer_0;

        private FloatBaseParam headXParam = new FloatBaseParam("HeadX");
        private FloatBaseParam headYParam = new FloatBaseParam("HeadY");
        private FloatBaseParam headZParam = new FloatBaseParam("HeadZ");

        public override void VRChat_OnUiManagerInit()
        {
            MelonCoroutines.Start(UpdateParamStores());
            MelonLogger.Msg(ConsoleColor.Cyan, "Initialized Sucessfully!");
        }

        IEnumerator UpdateParamStores()
        {
            for (; ; )
            {
                yield return new WaitForSeconds(2);
                headXParam.ResetParam();
                headYParam.ResetParam();
                headZParam.ResetParam();
            }
        }

        public override void OnUpdate()
        {
            //if (Input.GetKeyDown(KeyCode.T))
            //{
            //    MelonLogger.Msg("You just pressed T");
            //}

            //Vector3 test = VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Internal_IkController_0.HeadEffector.transform.position;
            //VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Internal_IkController_0.Head

            Transform test_transform = TransformOfBone(VRCPlayer.field_Internal_Static_VRCPlayer_0, HumanBodyBones.Head);
            //Vector3 test = test_transform.position;
            Vector3 test_eulerAngles = test_transform.eulerAngles;

            //MelonLogger.Msg("Head Rotation: X=" + test_eulerAngles.x + ", Y=" + test_eulerAngles.y + ", Z=" + test_eulerAngles.z);

            headXParam.ParamValue = (float)(test_eulerAngles.x / 360);
            headYParam.ParamValue = (float)(test_eulerAngles.y / 360);
            headZParam.ParamValue = (float)(test_eulerAngles.z / 360);

            //MelonLogger.Msg("Head Rotation: headXParam=" + headXParam.ParamValue + ", headYParam=" + test_eulerAngles.y + ", headZParam=" + test_eulerAngles.z);
        }

        public static Transform TransformOfBone(VRCPlayer player, HumanBodyBones bone)
        {

            Vector3 bonePosition = player.transform.position;
            VRCAvatarManager avatarManager = player.prop_VRCAvatarManager_0;
            //if (!avatarManager)
            //    return bonePosition;
            Animator animator = avatarManager.field_Private_Animator_0;
            //if (!animator)
            //    return bonePosition;
            Transform boneTransform = animator.GetBoneTransform(bone);
            //if (!boneTransform)
            //    return bonePosition;

            return boneTransform;
        }

        public static Vector3 CenterOfPlayer(VRCPlayer player)
        {

            Vector3 center = player.transform.position;
            Vector3 headPos = TransformOfBone(player, HumanBodyBones.Head).position;
            Vector3 lFootPos = TransformOfBone(player, HumanBodyBones.LeftFoot).position;

            return new Vector3(center.x, headPos.y - (Vector3.Distance(headPos, lFootPos) / 2f), center.z);
        }
    }
}
