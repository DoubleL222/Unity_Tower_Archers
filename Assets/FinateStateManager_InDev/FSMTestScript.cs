using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//WITH THIS FSM MANAGER YOU ONLY DEAL WITH YOUR PREDIFINED ENUM SETS
//DEFINE AN ENUM FOR STATES AND AN ENUM FOR EVENTS

//ENUM FOR EVENTS
//public enum enumEvents
//{
//    AtoB,
//    BtoA,
//    BtoC,
//    CtoB,
//    AtoC,
//    CtoA
//}
////ENUM FOR STATES
//public enum enumStates
//{
//    A,
//    B,
//    C
//}

public class FSMTestScript : MonoBehaviour {

 //   //CHANGE THIS EVENT FROM EDITOR AND INVOKE BY PRESSING ON THE BUTTON
 //   public enumEvents currentEvent;

 //   void Start () {
 //       MakeStateMachine();
 //   }

 //   //EXAMPLE FSM INIT
	//void MakeStateMachine()
 //   {

 //       //CREATE LIST OF STAT ENUMS
 //       List <enumStates> allStates = new List<enumStates>() { enumStates.A, enumStates.B, enumStates.C };
 //       //ADD TO MANAGER
 //       ManagerFSM.AddAllStates(allStates);

 //       //ADD EVENT CONSEQUENCES (_fromstate, _event, _toState)
 //       ManagerFSM.AddEventConsequence(enumStates.A, enumEvents.AtoB, enumStates.B);
 //       ManagerFSM.AddEventConsequence(enumStates.A, enumEvents.AtoC, enumStates.C);
 //       ManagerFSM.AddEventConsequence(enumStates.B, enumEvents.BtoA, enumStates.A);
 //       ManagerFSM.AddEventConsequence(enumStates.B, enumEvents.BtoC, enumStates.C);
 //       ManagerFSM.AddEventConsequence(enumStates.C, enumEvents.CtoA, enumStates.A);
 //       ManagerFSM.AddEventConsequence(enumStates.C, enumEvents.CtoB, enumStates.B);

 //       //ADD FUNCTIONS THAT YOU WISH TO CALL ON EXIT AND ON ENTER EVENTS
 //       ManagerFSM.AddOnExitCall(enumStates.A, OnAExit);
 //       ManagerFSM.AddOnEnterCall(enumStates.C, OnCEnter);

 //       //SET TO DESIRED FIRST STATE
 //       ManagerFSM.ForceState(enumStates.A);
 //   }
	//// Update is called once per frame
	//void Update () {
     
	//}

 //   //BUTTONCALLBACK
 //   public void INVOKESUMTN()
 //   {
 //       ManagerFSM.InvokeEvent(currentEvent);
 //   }

 //   //EXAMPLE
 //   public void OnAExit()
 //   {
 //       Debug.Log("ON AAAA EXIT DELEGATE CALLED");
 //   }
 //   public void OnCEnter()
 //   {
 //       Debug.Log("ON CCCCC ENTER DELEGATE CALLED");
 //   }
}
