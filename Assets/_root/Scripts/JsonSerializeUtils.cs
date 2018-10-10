using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonSerializeUtils : MonoBehaviour {

	public string[] Saludos, Conflicto, Despedidas;

	public class Dialogos {
        public string[] Saludos;
        public string[] Conflicto;
        public string[] Despedidas;
        public int zapato = 2;
	}

	void Start()
	{
        //Estos don dos archivos que me hice de ejemplo
        string DialogosP1 = "Assets/_root/Scripts/dialogos.json", DialogosP2 = "Assets/_root/Scripts/dialogos 2.json";

        Dialogos dialogos = CargaDialogos(DialogosP1);
        Saludos = dialogos.Saludos;
        Conflicto = dialogos.Conflicto;
        Despedidas = dialogos.Despedidas;
    }
	public Dialogos CargaDialogos(string filename) //estoy suponiendo habrà un archivo para los dialogos de cada personaje
	{
        //Creamos un reader que saque todo del archivo
        // "Assets/_root/Scripts/dialogos.json"
        StreamReader reader = new StreamReader(filename);
        // lo serializamos en un objeto tipo Dialogos
        Dialogos d = JsonUtility.FromJson<Dialogos>(reader.ReadToEnd());
        //cerramos el reader para liberar memoria
        reader.Close();

        return d;
	}


}
