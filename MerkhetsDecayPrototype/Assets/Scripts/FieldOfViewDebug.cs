//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(EnemyAI))]
//public class FieldOfViewDebug : Editor
//{
//    //from: https://www.youtube.com/watch?v=j1-OyLo77ss
//    private void OnSceneGUI()
//    {
//        EnemyAI enemyAI = (EnemyAI)target;
//        Handles.color = Color.white;
//        Handles.DrawWireArc(enemyAI.transform.position, Vector3.up, Vector3.forward, 360, enemyAI._fovRadius);

//        Vector3 viewAngle01 = DirectionFromAngle(enemyAI.transform.eulerAngles.y, -enemyAI._fovAngle / 2);
//        Vector3 viewAngle02 = DirectionFromAngle(enemyAI.transform.eulerAngles.y, enemyAI._fovAngle / 2);

//        Handles.color = Color.yellow;
//        Handles.DrawLine(enemyAI.transform.position, enemyAI.transform.position + viewAngle01 * enemyAI._fovRadius);
//        Handles.DrawLine(enemyAI.transform.position, enemyAI.transform.position + viewAngle02 * enemyAI._fovRadius);

//        //if (enemyAI._canSeePlayer)
//        //{
//        //    Handles.color = Color.green;
//        //    Handles.DrawLine(enemyAI.transform.position, enemyAI._player.transform.position);
//        //}
//    }

//    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
//    {
//        angleInDegrees += eulerY;

//        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
//    }
//}
