## Objetivo

Jogo competitivo entre dois jogadores, ao qual cada um controla um exercito e tem como objetivo derrotar o rei inimigo em campo. É um jogo misto de combate tático que é usado cartas e ação dentro de um board.

## Referencies

[Referencias - Inspiração](https://www.notion.so/Referencias-Inspira-o-82427f5612644a12998b591ebe0755cf?pvs=21)

[Evolution](https://www.notion.so/Evolution-de71b5d40fba449f97ba8069658a447b?pvs=21)

# Como jogar

O jogo consiste em um tabuleiro com o padrão 16x16 (podendo variar dependendo da configuração de jogo), cada jogador comanda um rei e seu exercito. 

### Preparação

No inicio do jogo cada jogador recebe 7 cartas de forma aleatória. (não existirá um deck para cada jogador mas sim um único para a partida onde os jogadores iram compartilhar a compra, assim deixando o jogo mais competitivo e baseado em sorte). 

### Fases do jogo

- Jogadores começam com 7 cartas aleatórias
- Na fase de preparação cada jogador pode chamar (ativar a carta como se fosse sumonar no board) até 3 cartas, sendo elas: Tropa, Terreno, Comando ou Equipamento, com isso se prepara para a batalha.
    - Nesse momento de preparação o jogador seleciona e posiciona suas 3 miniaturas, assim que todos finalizarem essa etapa as miniaturas ficaram visíveis para todos os jogadores.
- Será selecionado o primeiro jogador de forma aleatória.
- O jogador pode escolher fazer todas ou alguns dos passos citados a baixo, sendo que se ele fizer todos o fim do turno será  automático.  Os passos são:
    1. Comprar uma carta.
    2. Chamar uma carta para o campo de batalha (sendo qualquer tipo: Tropa, Terreno, Comando ou Equipamento)
    3. Efetua ação nas miniaturas do board, entre elas:  mover tropas ou atacar.
    4. Finaliza o turno.
    

### Condições de Win/Lose

O jogador ganha quando matar o rei inimigo.  Uma outra forma de ganhar é quando o inimigo lança bandeira branca, que é nada mais que desistir da batalha.

# Especificações - Details

## Campo de batalha

O campo de batalha é uma grid dependendo da configuração de jogo pode variar o tamanho, mas o padrão é 16x16. Onde na parte superior fica as tropas e o rei inimigo, já na parte inferir fica as tropas e rei do jogador.

Dentro do campo de batalha é dividido em 3 regiões como podemos ver na imagem abaixo, que são: faixa superior (marcado de vermelho), faixa inferior (marcado de azul) e faixa central (marcado de amarelo). As faixas servem para desmarcar onde pode ser feito determinadas açoes, que são: faixa vermelha é onde o inimigo pode adicionar seus exercitos e recursos assim como a faixa azul que é onde o jogador colocará as dele, já a faixa maior a amarela é onde a os dois geralmente se encontram, mas nenhum deles podem spawnar exercito lá. Já os pontos pretos são as posiçoes dos reis.

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/f99a8d8a-26be-49a3-8986-e5d96f9b6860/2a1c2700-4f2a-4c0b-b8e1-af0e57c68c0c/Untitled.png)

## Deck

Sobre o deck, normalmente vemos jogos ao qual cada jogador cria seu próprio deck e prepara ele para poder lutar contra seus amigos e colegas, mas aqui não será assim, o jogador sempre terá que mostrar que consegue se virar com o que tem na mão, ou seja, existirá apenas um deck para a partida, onde os jogadores compartilharam o deck durante o jogo.

Dependendo da configuração da partida o deck pode mudar e assim modificando a game play e dificuldade.

### Tipos de cartas

Dentro do jogo temos alguns tipos de cartas que possuem suas próprias características que podem mudar o andamento da game play, elas são:

- `Tropa` - São cartas para adicionar soldados ou variações do mesmo como arqueiros, basicamente são as pessoas que iram lutar por você, o seu povo.
- `Terreno` - São cartas que possuem o poder de mudar o terreno (apenas na faixa amarela)
- `Comando` - São cartas que ampliam a movimentação e organização de tropas e equipamentos.
- `Equipamento` - São cartas de auxilio que podem atacar, defender ou dar bônus.

### Cartas do tipo Tropa

Cartas do tipo tropa tem algumas subdivisões internas tanto na quantidade quanto no comportamento, elas são:

| Nome | Descrição | ATK/DEF/MOV | D-ATK |
| --- | --- | --- | --- |
| Arqueiro | Unidade de longo alcance | 1/1/2 | 3 |
| Soldado | Unidade de corpo a corpo | 2/2/1 | 1 |
| Lanceiro | Unidade de médio alcance | 2/2/1 | 2 |
| Cavalaria | Unidade montada | 2/3/4 | 1 |

Legenda: 

- ATK - ataque
- DEF - defesa
- MOV - movimentação
- D-ATK - Distancia de Ataque

**Tropas com marcador de Grupo**

Nas cartas de tropas podemos encontrar a marcação de “grupo” que significa que ao invés de adicionar apenas 1 unidade, será adicionada 3. Por exemplo, ao comprar uma carta de soldado e nela possuir o marcador `GRUPO` será adicionado 3 soldados alinhados horizontalmente.

### Cartas de tipo Terreno

Cartas desse tipo tem o poder de mudar o campo de batalha assim auxiliando na sua estratégia de guerrilha. Dentre as cartas de terreno temos as seguintes comportamentos:

- Mudança temporária - que é a mudança que dura entre 2 - 4 turnos.
- Mudança Permanente -  que fica até o fim do jogo.

**Mudança Temporária**

As mudanças desse tipo podem afetar o todo o terreno ou áreas do terreno, assim trazendo dinamismo nas estratégias.

| Nome | Efeito |
| --- | --- |
| Chuva | Durante 3 turnos o chão ficará encharcado, assim dificultando a movimentação (MOV -1) de qualquer exercito dentro da área de 6x3 |
| Brisa Feroz | Durante o turno, todos o exercito dentro da área de 6x2 será emburrado para trás 2x (andará para trás 2 casas) |
| Neblina | Durante 1 turno, todos os jogadores receberam DEF -1 e MOV -1 |
| Ensolarado | Durante 2 turnos o exercito dentro da área 2x1 recebe ATK +1 |

**Mudança Permanente**

As mudanças desse tipo ficam fixas até o fim da partida e podem ser sobrepostas por outras ou anuladas por cartas de anulação climática.

| Nome | Efeito |
| --- | --- |
| Nevoeiro | A distancia de ataques acima de 2 recebem D-ATK -1 |
| Lamaçal | O movimento de todo exercito que possui movimento acima de 2 é reduzido para MOV -1 |
| Tempestade | Condições de clima horrível tem um dano considerável na batalha, Reduz MOV -1, ATK -1, D-ATK -1 |

### Cartas do tipo Comando

Esse tipo de carta é responsável pela estratégia em si, ela possibilita fazer organizações de tropas e equipamentos. Comandos tem a possibilidade de dar mais dinamismo a batalha e fazer com que qualquer coisa possa mudar rapidamente.

| Nome | Efeito |
| --- | --- |
| Reagrupar Tropa | Chama todos o exercito para sua safe zone (faixa azul) |
| Corredor | Faz com que a unidade de tropa tenha MOV +2 |
| Realocar | Permite mover o equipamento de lugar MOV +5 |
| Realocação Maestral | Pode mover todos os equipamentos para qualquer lugar |
| Empurrar | Pode mover um equipamento leve MOV +2 |
| Concertar | Restaura um equipamento destruído |

### Cartas do tipo Equipamento

Cartas desse tipo podem ajudar muito durante a batalha, elas servem para auxiliar o comandante no avanço contra as tropas inimigas. Os equipamentos são divididos em 3 tipos:

- Equipamentos de ataque  - equipamentos que de fato tem uma ação ofensiva contra os inimigos, assim causando dano.
- Equipamentos de Defesa - São artimanhas usadas para controlar a movimentação do inimigo.
- Equipamentos de moral -  São equipamentos que dão bônus para a tropa.

Mas não se engane, os equipamentos podem ser destruídos pelo inimigo e tudo que eles proporcionaram é revogado!

OBS::.

- Todos os efeitos por equipamento de moral é acumulável.
- Cartas de equipamentos podem ser adicionados tanto na zona do jogador quanto na zona de batalha, mas jamais na zona inicial do inimigo.

**Equipamentos de Ataque**

| Nome | Efeito | ATK/DEF/D-ATK | Tamanho |
| --- | --- | --- | --- |
| Lança Dardos | lançador que dispara no inimigo a sua frente | 1/1/2 | Leve |
| Torre de Arquearia | Torre alta onde arqueiros pode ser vistos atacando. | 1/2/3 | Pesado |
| catapulta | Catapulta lançadora de pedras | 3/2/5 | Pesado |

**Equipamentos de Defesa**

| Nome | Efeito | ATK/DEF/D-ATK | Tamanho |
| --- | --- | --- | --- |
| Cerco de Madeira | Barreira que impede a movimentação do inimigo tamanho 2x1 | 0/1/0 | Leve |
| Trincheira | Barreira cavada no chão que impede a movimentação inimiga com uma área de 2x1 | 0/1/0 | Leve |
| Murro | Um murro feito de madeira com área de 4x1 | 0/2/0 | Pesado |

**Equipamentos de Moral**

| Nome | Efeito | Tamanho |
| --- | --- | --- |
| Mesa de banquete | Mesa com comida para o exercito, aumenta ATK +1 para os soldados. | Leve |
| Estabulo | Estabulo para os cavalos, toda cavalaria recebe MOV +1 | Pesado |
| Ferreiro | Soldado e Lanceiro recebe ATK +1 DEF +1 | Pesado |
| Campo de treino ao alvo | Arqueiros recebem D-ATK +1 | Pesado |
| Engenharia | Equipamentos de ataque recebem ATK +1 | Pesado |
| Enfermaria | Soldados recebem DEF +1 | Pesado |

## Rei

A entidade mais importante do jogo é o rei, ele que trás toda a tropa para a batalha e faz com que eles se movimentem e atinjam alvos específicos para garantir a vitória.

O rei é como qualquer outro soldado possuindo seus atributos se necessário lutando para sobreviver.

| Name | ATK/DEF/MOV | D_ATK |
| --- | --- | --- |
| Rei | 2/20/1 | 1 |