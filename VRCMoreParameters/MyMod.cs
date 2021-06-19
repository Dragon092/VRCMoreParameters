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

        private readonly FloatBaseParam headXParam = new FloatBaseParam("HeadX");
        private readonly FloatBaseParam headYParam = new FloatBaseParam("HeadY");
        private readonly FloatBaseParam headZParam = new FloatBaseParam("HeadZ");

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

                //MelonLogger.Msg("headYParam.ParamIndex: " + headYParam.ParamIndex + ", headYParam.ParamValue:" + headYParam.ParamValue);
            }
        }

        public override void OnUpdate()
        {
            //Vector3 test = VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Internal_IkController_0.HeadEffector.transform.position;
            //VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Internal_IkController_0.Head

            Transform head_bone_transform = TransformOfBone(VRCPlayer.field_Internal_Static_VRCPlayer_0, HumanBodyBones.Head);
            //Vector3 test = head_bone_transform.position;
            Vector3 head_bone_eulerAngles = head_bone_transform.eulerAngles;

            //MelonLogger.Msg("Head Rotation: X=" + head_bone_eulerAngles.x + ", Y=" + head_bone_eulerAngles.y + ", Z=" + head_bone_eulerAngles.z);

            headXParam.ParamValue = (float)(NormalizeFloat(0, 360, 0, 1, ((head_bone_eulerAngles.x + 180) % 360))); // TODO: Test
            headYParam.ParamValue = (float)(NormalizeFloat(0, 360, 0, 1, ((head_bone_eulerAngles.y + 180) % 360))); // TODO: Test
            headZParam.ParamValue = (float)(NormalizeFloat(0, 360, 0, 1, ((head_bone_eulerAngles.z + 180) % 360))); // Should be working

            //MelonLogger.Msg("Head Rotation: headXParam=" + headXParam.ParamValue + ", headYParam=" + headYParam.ParamValue + ", headZParam=" + headZParam.ParamValue);
        }

        private static float NormalizeFloat(float minInput, float maxInput, float minOutput, float maxOutput, float value)
        {
            return (maxOutput - minOutput) / (maxInput - minInput) * (value - maxInput) + maxOutput;
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

        /*
        public static Vector3 CenterOfPlayer(VRCPlayer player)
        {

            Vector3 center = player.transform.position;
            Vector3 headPos = TransformOfBone(player, HumanBodyBones.Head).position;
            Vector3 lFootPos = TransformOfBone(player, HumanBodyBones.LeftFoot).position;

            return new Vector3(center.x, headPos.y - (Vector3.Distance(headPos, lFootPos) / 2f), center.z);
        }
        */
    }
}
