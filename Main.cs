using MelonLoader;
using HarmonyLib;
using Il2Cpp;
[assembly: MelonInfo(typeof(NoctuaryMod.SuperBlessings), "Noctuary-SuperBlessings", "1.0.0-beta", "洛灯夏夜(Xia)", "https://xiau.net")]
[assembly: MelonGame]
namespace NoctuaryMod
{
    public class SuperBlessings : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("╔══════════════════════════════════════╗");
            LoggerInstance.Msg("║  Noctuary-SuperBlessings v1.0.0-beta ║");
            LoggerInstance.Msg("║  作者: 洛灯夏夜(Xia)                 ║");
            LoggerInstance.Msg("║  网站: https://xiau.net              ║");
            LoggerInstance.Msg("╚══════════════════════════════════════╝");
            LoggerInstance.Msg("注入成功，此夜已获得月灯辉夜的全部祝福之花权能");
        }

        // ============================
        // 修改祝福基础消耗为 0
        // ============================
        [HarmonyPatch(typeof(BlessConfig), "get_BlessCostBasic")]
        static class Patch_Cost
        {
            // Postfix：在原函数执行后修改返回值
            static void Postfix(ref int __result) => __result = 0;
        }

        // ============================
        // 修改最大槽位为999
        // ============================
        [HarmonyPatch(typeof(BlessManager), "get_Notch")]
        static class Patch_Notch
        {
            static void Postfix(ref int __result) => __result = 999;
        }

        // ============================
        // 修改剩余槽位为 999
        // ============================
        [HarmonyPatch(typeof(BlessManager), "get_LeftNotch")]
        static class Patch_LeftNotch
        {
            static void Postfix(ref int __result) => __result = 999;
        }

        // ============================
        // 强制允许装备任何祝福
        // ============================
        [HarmonyPatch(typeof(BlessManager), "CanEquip")]
        static class Patch_CanEquip
        {
            static void Postfix(ref bool __result) => __result = true;
        }

        // ============================
        // 禁用 COSTUI 更新
        // ============================
        [HarmonyPatch(typeof(BlessUI), "UpdateCost")]
        static class Patch_UpdateCost
        {
            // Prefix 返回 false 跳过原函数执行
            static bool Prefix() => false;
        }

        // ============================
        // 强制装备祝福
        // ============================
        [HarmonyPatch(typeof(BlessManager), "ReqEquipBless")]
        static class Patch_ForceEquip
        {
            static bool Prefix(BlessManager __instance, string blessId)
            {
                // 获取当前已装备祝福列表
                var list = __instance.Curbless;

                // 如果列表为空，继续执行原方法
                if (list == null) return true;

                // 如果已经装备过，阻止重复装备
                if (list.Contains(blessId)) return false;

                // 强制添加祝福
                list.Add(blessId);

                // 手动触发装备事件
                __instance.BlessEquipedEvent?.Invoke(blessId);

                // 返回 false 阻止原函数执行
                return false;
            }
        }
    }
}