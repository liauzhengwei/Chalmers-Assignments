import numpy as np
import sys
from matplotlib import pyplot as plt

class FeedforwardNN(object):
    """
    A simple implementation of a feedforward neural network for classification
    and regression.
    """

    def __init__(self, n_iter=500, eta=0.001, n_hidden=8, regression=False,
                 relu=False, plot=False, verbose=True):
        self.n_iter = n_iter
        self.eta = eta
        self.n_hidden = n_hidden
        self.regression = regression
        self.relu = relu
        self.plot = plot
        self.verbose = verbose

    def find_classes(self, Y):
        classes = sorted(set(Y))
        if len(classes) != 2:
            raise Exception("this does not seem to be a 2-class problem")
        self.positive_class = classes[0]
        self.negative_class = classes[1]

    def encode_outputs(self, Y):
        return np.array([1 if y == self.positive_class else -1 for y in Y])

    def predict(self, X):
        H = self.compute_hidden(X.dot(self.w1) + self.w1_i)
        O = self.compute_output(H.dot(self.w2) + self.w2_i)
        if self.regression:
            return O
        else:
            return np.select([O >= 0.5, O < 0.5],
                            [self.positive_class,
                             self.negative_class])

    def compute_loss(self, o, y):
        """
        Computes the squared error loss (for regression) or the log loss
        (for classification).
        """
        if self.regression:
            return (o - y)**2
        else:
            if y > 0:
                return -np.log(o)
            else:
                return -np.log(1-o)

    def gradient_loss_and_output(self, a_o, y):
        """
        Gradient of the loss and output activation.
        """
        if self.regression:
            return 2*(a_o - y)
        else:
            return -y / (1 + np.exp(y * a_o))

    def compute_output(self, a_o):
        """
        Applies the output activation, to compute the
        output layer. The input a_o is a linear transformation
        applied to the outputs of the hidden layer.

        The output activation is just a "pass-through" for regression,
        and a sigmoid for classification (which means that the output
        is a probability).
        """
        if self.regression:
            return a_o
        else:
            return 1/(1+np.exp(-a_o))

    def compute_hidden(self, a_h):
        """
        Applies the hidden-layer activation, to compute the outputs
        of the hidden layer. The input a_h is a linear transformation
        applied to the input feature vector.

        The hidden-layer activation is either a rectified linear unit
        (ReLU) or hyperbolic tangent (tanh).
        """
        if self.relu:
            return a_h*(a_h > 0)
        else:
            return np.tanh(a_h)

    def gradient_hidden(self, h):
        """
        Computes the gradient of the hidden-layer activation
        function.
        """
        if self.relu:
            return 1.0*(h > 0)
        else:
            return 1 - h * h

    def fit(self, X, Y):
        """
        Train the neural network model.
        """

        # If necessary, convert the input to a NumPy matrix.
        if not isinstance(X, np.ndarray):
            X = X.toarray()

        # For classification, we code the outputs as +1 or -1.
        if not self.regression:
            self.find_classes(Y)
            Y = self.encode_outputs(Y)

        np.random.seed(0)

        n_features = X.shape[1]

        # Initialize the weights to small random values.
        # w1 are the weights for the hidden layer and w2 for the output layer.
        # w1_i are the "intercept" weights in the hidden layer, w2_i for the output layer.
        self.w1 = 0.1 * np.random.normal(size=(n_features, self.n_hidden))
        self.w1_i = 0.5 * np.random.normal(size=self.n_hidden)
        self.w2 = 0.1 * np.random.normal(size=self.n_hidden)
        self.w2_i = 0.5 * np.random.normal()

        # We keep track of the loss values, in order to plot them at the end.
        loss_history = []

        for i in range(1, self.n_iter+1):

            if self.verbose and i % 10 == 0:
                print('Epoch {}.'.format(i))

            loss_sum = 0

            # For each training instance...
            for x, y in zip(X, Y):

                # Compute the scores in the hidden layer.
                a_h = x.dot(self.w1) + self.w1_i
                h = self.compute_hidden(a_h)

                # The output score.
                a_o = h.dot(self.w2) + self.w2_i
                o = self.compute_output(a_o)

                # Compute the squared error loss.
                loss_sum += self.compute_loss(o, y)

                # Gradient with respect to the weights in the second layer.
                g_loss = self.gradient_loss_and_output(a_o, y)
                dw2 = g_loss*h
                dw2_i = g_loss

                # Backpropagate to the first layer.
                # First some intermediate steps.
                bp1 = g_loss*self.w2
                bp2 = bp1 * self.gradient_hidden(h)

                # Gradient with respect to the weights in the first layer.
                # np.outer means an "outer product": a matrix product of a
                # column vector and row vector.
                dw1 = np.outer(x, bp2)
                dw1_i = bp2

                # SGD updates of the weights in both layers.
                self.w2 -= self.eta * dw2
                self.w1 -= self.eta * dw1

                self.w2_i -= self.eta * dw2_i
                self.w1_i -= self.eta * dw1_i

            if self.verbose and i % 10 == 0:
                print('Mean loss in epoch: {:.4f}'.format(loss_sum/X.shape[0]))
            if self.plot:
                loss_history.append(loss_sum/X.shape[0])


        if self.plot:
            plt.plot(loss_history)
            plt.xlabel('epoch')
            plt.ylabel('mean loss')
            plt.show()


