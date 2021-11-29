using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider2D))]
public class Snake : MonoBehaviour
{
    //uma lista que guarda todos segmentos da cobrinha.
    private List<Transform> _segments = new List<Transform>();
    //localização de cada segmento.
    public Transform segmentPrefab;
    //direção de todos apontado para direita, pra não quebrar o formato da cobrinha.
    public Vector2 direction = Vector2.right;
    //tamanho inicial da cobra = 4
    public int initialSize = 4;
    public float velocidade = 0.5f;

    //assim que o jogo inicia, liga a função start.
    private void Start()
    {
        //ativa a função que reinicia a cobrinha para iniciar o jogo com o tamanho inicial.
        ResetState();
        Time.timeScale = velocidade;
    }
    
    //atualiza a cada frame
    private void Update()
    {
        //se estiver andando na direção x, aguarda inputs para mover para cima, ou para baixo.
        if (this.direction.x != 0f)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("MoveVertical") < -0.7) {
                this.direction = Vector2.up;
            } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("MoveVertical") > 0.7) {
                this.direction = Vector2.down;
            }
        }
        //se estiver andando na direção y, aguarda inputs para mover para direita ou esquerda.
        else if (this.direction.y != 0f)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("MoveHorizontal") > 0.7) {
                this.direction = Vector2.right;
            } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("MoveHorizontal") < -0.7) {
                this.direction = Vector2.left;
            }
        }
    }

    private void FixedUpdate()
    {
        //atualiza a posição de cada segmento do corpo da cobrinha usando for.
        for (int i = _segments.Count - 1; i > 0; i--) {
            _segments[i].position = _segments[i - 1].position;
        }

        // arredonda a localização de cada segmento da cobrinha para o número inteiro mais próximo, somando com a direção que ela está se movendo.
        float x = Mathf.Round(this.transform.position.x) + this.direction.x;
        float y = Mathf.Round(this.transform.position.y) + this.direction.y;

        this.transform.position = new Vector2(x, y);
    }
    
    //função faz a cobrinha ganhar mais um segmento.
    public void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);
        //deixa o jogo um pouco mais rápido.
        velocidade = (velocidade + 0.01f);
    }
    
    //função reinicia o tamanho e local da cobrinha.
    public void ResetState()
    {
        //cobrinha começa no centro do mapa apontada para a direita.
        this.direction = Vector2.right;
        this.transform.position = Vector3.zero;

        // destrói todos segmentos da cobrinha que vão além do tamanho inicial inserido no InitialSize (4).
        for (int i = 1; i < _segments.Count; i++) {
            Destroy(_segments[i].gameObject);
        }

        // limpa a lista de todos segmentos da cobrinha, e insere os segmentos novos
        _segments.Clear();
        _segments.Add(this.transform);

        // cresce a cobrinha para chegar ao tamanho inicial.
        for (int i = 0; i < this.initialSize - 1; i++) {
            Grow();
        }
        //reinicia a velocidade da cobrinha.
        velocidade = 0.5f;
    }

    //assim que colide com algum trigger, liga a função
    private void OnTriggerEnter2D(Collider2D other)
    {
        //se o colisor possuir a tag de comida, liga a função grow para a cobrinha crescer.
        if (other.tag == "Food") {
            Grow();
        } 
        //se não possuir a tag comida, e sim obstáculo, a cobrinha é reiniciada através da função.
        else if (other.tag == "Obstacle") {
            ResetState();
        }
    }

    //como configurar o controle foi encontrado em https://www.youtube.com/watch?v=TqXHlTeyvgo.
}
