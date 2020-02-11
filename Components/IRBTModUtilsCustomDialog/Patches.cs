﻿using System;
using BattleTech;
using BattleTech.UI;
using Harmony;

// thank you Frosty IRBTModUtils CustomDialog
// https://github.com/IceRaptor/IRBTModUtils
namespace PanicSystem.Components.IRBTModUtilsCustomDialog {

    // Register listeners for our events, using the CombatHUD hook
    [HarmonyPatch(typeof(CombatHUD), "SubscribeToMessages")]
    public static class CombatHUD_SubscribeToMessages {
        public static void Postfix(CombatHUD __instance, bool shouldAdd) {
            if (__instance != null) {
                __instance.Combat.MessageCenter.Subscribe(
                    (MessageCenterMessageType)MessageTypes.OnCustomDialog, new ReceiveMessageCenterMessage(Coordinator.OnCustomDialogMessage), shouldAdd);
            }
        }
    }

    // Initialize shared elements (CombatGameState, etc)
    [HarmonyPatch(typeof(CombatHUD), "Init")]
    [HarmonyPatch(new Type[] {  typeof(CombatGameState) })]
    public static class CombatHUD_Init {
        public static void Postfix(CombatHUD __instance, CombatGameState Combat) {
            Coordinator.OnCombatHUDInit(Combat, __instance);
        }
    }

    // Teardown shared elements to prevent NREs
    [HarmonyPatch(typeof(CombatHUD), "OnCombatGameDestroyed")]
    public static class CombatHUD_OnCombatGameDestroyed {
        public static void Prefix() {
            Coordinator.OnCombatGameDestroyed();
        }
    }
}
