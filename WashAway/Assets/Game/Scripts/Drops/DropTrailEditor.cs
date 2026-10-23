using UnityEditor;

[CustomEditor(typeof(DropTrail))]
public class DropTrailEditor : Editor
{
    private DropTrail dropTrail;

    void OnEnable()
    {
        dropTrail = (DropTrail)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var nextProperty = serializedObject.GetIterator();
        nextProperty.Next(true);

        while (nextProperty.NextVisible(false))
        {
            switch (nextProperty.name)
            {
                case "m_Script":
                    break;
                default:
                    EditorGUILayout.PropertyField(nextProperty);
                    break;
            }
        }

        if(serializedObject.ApplyModifiedProperties())
        {
            dropTrail.RebuildMesh();
        }
    }
}
