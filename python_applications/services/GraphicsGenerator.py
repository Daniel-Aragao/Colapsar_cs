import math

class GraphicsGenerator:
    def __init__(self, x: []):
        self.x = x
        self.y = []
        self.legends = []

    def add_y(self, y):
        self.y.append(y)
        return self
    
    def add_legend(self, legend):
        self.legends.append(legend)
        return self

    @staticmethod
    def __apply_log(arr):
        return [math.log(item) for item in arr]

    @staticmethod
    def __apply_func(arr, func):
        return [func(item) for item in arr]

    def log_x(self):
        self.x = GraphicsGenerator.__apply_log(self.x)        
        return self
    
    def log_y(self):
        for i, arr in enumerate(self.y):
            self.y[i] = GraphicsGenerator.__apply_log(arr)

        return self

    def apply_func_x(self, func):
        self.x = GraphicsGenerator.__apply_func(self.x, func)
        return self
    
    def apply_func_y(self, func):
        self.y = GraphicsGenerator.__apply_func(self.y, func)
        return self
        
    def build_plt(self, plt):
        for i, arr in enumerate(self.y):
            if self.legends:
                plt.plot(self.x, arr, label=self.legends[i])
            else:
                plt.plot(self.x, arr)
                
