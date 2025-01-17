﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AplicadaFinal
{

    public partial class Caja : System.Web.UI.Page
    {

        public empleado LogEmpleado
        {
            get
            {
                if (Session["Empleado"] == null)
                    Session["Empleado"] = new empleado();
                return (empleado)Session["Empleado"];
            }

            set
            {
                Session["Empleado"] = value;
            }
        }

        public List<orden> LOrden
        {
            get
            {
                if (Session["orden"] == null)
                    Session["orden"] = new List<orden>();
                return (List<orden>)Session["orden"];
            }

            set
            {
                Session["orden"] = value;
            }
        }

        public orden Ordenn
        {
            get
            {
                if (Session["LaOrden"] == null)
                    Session["LaOrden"] = new orden();
                return (orden)Session["LaOrden"];
            }

            set
            {
                Session["LaOrden"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (LogEmpleado.id_tipo != 3)
            {
                Server.Transfer("Default.aspx");
            }
      
            Buscadores bus = new Buscadores();
            List<ordenestado> Lordenestado = bus.buscarListOrdenEstado(3);
            List<orden> Lorden = bus.buscarordeestado(Lordenestado);
            List<vehiculo> LVehiculo = bus.buscarordevehiculo(Lorden);
            LOrden = Lorden;
        }

        protected void BtnBuscarO(object sender, EventArgs e)
        {
            Buscadores bus = new Buscadores();
            lblmodelo.Text = "La patente ingresada no se encuentra para Cobrar";
            lblpatente.Text = "-";
            lblprecio.Text = "-";
            GridView1.DataSource = null;
            GridView1.DataBind();
            if (txtorden.Value != "")
            {
                txtpatente.Visible = false;
                btnbuscarpatente.Visible = false;
                foreach (orden oOrden in LOrden)
                {
                    if (int.Parse(txtorden.Value) == oOrden.id_orden)
                    {
                        Ordenn = oOrden;
                        lblpatente.Text = "PATENTE: " + oOrden.vehiculo.patente;
                        modelo omodelo = bus.buscarmodelo(oOrden.vehiculo);
                        lblmodelo.Text = "MODELO: " + omodelo.nombre;
                        CargarGrid(oOrden);
                    }
                }
            }
        }

        protected void BtnBuscarP(object sender, EventArgs e)
        {
            Buscadores bus = new Buscadores();
            lblmodelo.Text = "La patente ingresada no se encuentra para Cobrar";
            lblpatente.Text = "-";
            GridView1.DataSource = null;
            GridView1.DataBind();
            lblprecio.Text = "-";
            if (txtpatente.Value != "")
            {
                txtorden.Visible = false;
                btnbuscarorden.Visible = false;
                foreach (orden oOrden in LOrden)
                {
                    if (txtpatente.Value == oOrden.vehiculo.patente)
                    {
                        Ordenn = oOrden;
                        lblpatente.Text = "PATENTE: " + oOrden.vehiculo.patente;
                        modelo omodelo = bus.buscarmodelo(oOrden.vehiculo);
                        lblmodelo.Text = "MODELO: " + omodelo.nombre;
                        CargarGrid(oOrden);
                    }
                }
            }
        }

        private void CargarGrid(orden oOrden)
        {
            int a = 0;
            Buscadores bus = new Buscadores();
            List<ordenservicio> Lidservidcios = new List<ordenservicio>();
            Lidservidcios = bus.buscarlistaid(oOrden.id_orden);
            List<servicio> Lservicios = ObtenerServicios(Lidservidcios);
            foreach (servicio x in Lservicios)
            {
                a = a + int.Parse(x.precio);
            }
            lblprecio.Text = "PRECIO TOTAL: " + a;
            GridView1.DataSource = Lservicios;
            GridView1.DataBind();
        }

        private List<servicio> ObtenerServicios(List<ordenservicio> Lidservidcios)
        {
            List<servicio> Lservicio = new List<servicio>();
            Buscadores bus = new Buscadores();
            foreach (ordenservicio idservicios in Lidservidcios)
            {
                servicio oservicio = bus.buscarservicio(idservicios.id_servicio);
                Lservicio.Add(oservicio);
            }
            return Lservicio;
        }

        protected void BtnCobrar(object sender, EventArgs e)
        {

            using (aplicadaBDEntities DBF = new aplicadaBDEntities())
            {
                ordenestado oestado = (from q in DBF.ordenestado where q.id_orden == Ordenn.id_orden select q).First();
                oestado.estado = 4;
                oestado.fecha = System.DateTime.Now;
                DBF.SaveChanges();
                Server.Transfer("Caja.aspx");
            }
        }

    }

}