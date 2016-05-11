class Cell(object):
    def __init__(self, upper, side):
        self.upper = upper
        self.side = side


def list_of_cells(n):
    cells = []
    for _ in range(n):
        cells.append(Cell(True, True))
    return cells


def get_maze(n):
    maze = []
    for _ in range(n):
        maze.append(list_of_cells(n))
    return maze


import pygame, sys
from pygame.locals import *


def draw_maze(maze):
    # set up the window
    surface = pygame.display.set_mode((500, 400), 0, 32)

    CELL_SIZE = 20

    BLACK = (0, 0, 0)
    WHITE = (255, 255, 255)

    # draw the white background onto the surface
    surface.fill(WHITE)

    for y_index, row in enumerate(maze):
        for x_index, cell in enumerate(row):
            cell_top_left = (x_index * CELL_SIZE, y_index * CELL_SIZE)
            if cell.upper:
                end = (cell_top_left[0] + CELL_SIZE, cell_top_left[1])
                pygame.draw.line(
                    surface, BLACK, cell_top_left, end, 1)
            if cell.side:
                end = (cell_top_left[0], cell_top_left[1] + CELL_SIZE)
                pygame.draw.line(
                    surface, BLACK, cell_top_left, end, 1)

    maze_top_left = (0, 0)
    # Can just borrow the final values of y_index and x_index
    maze_bottom_right = ((y_index + 1) * CELL_SIZE, (x_index + 1) * CELL_SIZE)

    pygame.draw.rect(surface, BLACK, (maze_top_left, maze_bottom_right), 1)
    # draw the window onto the screen
    pygame.display.update()

    # run the game loop
    while True:
        for event in pygame.event.get():
            if event.type == QUIT:
                pygame.quit()
                sys.exit()



def main():
    n = 10
    maze = get_maze(n)
    draw_maze(maze)


if __name__ == '__main__':
    main()
