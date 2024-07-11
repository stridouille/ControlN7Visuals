using Plots
using Plots.PlotMeasures
using Images

#=
#astroid curve
x(t) = sin(t)^3
y(t) = cos(t)^3
plot(x, y, 0, 2*pi)
=#

#sinus curve
x(t) = 0
plot(x, -pi, pi, linecolor=:red)
x(t) = pi/4
plot!(x, -pi, pi, linecolor=:red)
x(t) = -pi/4
plot!(x, -pi, pi, linecolor=:red)
vline!([-pi/4, 0, pi/4], linecolor=:red)



#use these attributes
plot!(dpi = 1000, framestyle=:none, fmt=:png, background=false, legend=false, grid=false, axis=false, border=false, size=(1000,500), margin=0mm)
scatter!((-pi, -pi/2), color = "white", label = "", markersize = 0.1)
scatter!((pi, pi/2), color = "white", label = "", markersize = 0.1)

#save the plot in desired path
img_path = "doc/img/help.png"
savefig(img_path)

#these lines crop the image's margins
img = load(img_path)
img = img[208:4639, 749:9659]
save(img_path, img)