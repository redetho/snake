using UnityEngine;

public class Food : MonoBehaviour
{
    //script de comida
    public Collider2D gridArea;

    private void Start()
    {
        //inicia a função de colocar uma comida em uma área aleatória dentro da tela.
        RandomizePosition();
    }

    public void RandomizePosition()
    {
        // limites da área onde a comida pode ser colocada
        Bounds bounds = this.gridArea.bounds;

        //localização x e y onde a comida será colocada, dentro dos limites (bounds)
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        // arredonda os números escolhidos no número inteiro mais próximo. se for com final x.5, o número será arredondado para o número par mais próximo.
        x = Mathf.Round(x);
        y = Mathf.Round(y);

        this.transform.position = new Vector2(x, y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //assim que a cobrinha colidir com o trigger (tocar na comida), a comida é invocada em outro lugar.
        RandomizePosition();
    }

}
