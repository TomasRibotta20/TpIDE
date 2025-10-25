# ?? GUÍA RÁPIDA DE ESTILOS - SISTEMA ACADÉMICO

## ?? ARCHIVO DE ESTILOS COMPARTIDOS

Usa la clase `FormStyles` para aplicar estilos consistentes rápidamente:

```csharp
using WIndowsForm;

// Aplicar estilo al formulario
FormStyles.ApplyFormStyle(this, "Titulo del Formulario");

// Crear header
var header = FormStyles.CreateHeaderPanel("Titulo Principal", "Subtitulo opcional");
this.Controls.Add(header);

// Crear botones con colores semánticos
var btnNuevo = FormStyles.CreateSuccessButton("Nuevo", BtnNuevo_Click);
var btnEditar = FormStyles.CreatePrimaryButton("Editar", BtnEditar_Click);
var btnEliminar = FormStyles.CreateDangerButton("Eliminar", BtnEliminar_Click);

// Estilizar DataGridView
FormStyles.StyleDataGridView(miDataGridView);
```

---

## ?? PALETA DE COLORES RÁPIDA

### Colores por Función:

| Función | Color | RGB | Uso |
|---------|-------|-----|-----|
| **Primary** | ?? Azul | `41, 128, 185` | Headers, botones principales |
| **Success** | ?? Verde | `46, 204, 113` | Crear, Guardar, Confirmar |
| **Danger** | ?? Rojo | `231, 76, 60` | Eliminar, Cancelar, Cerrar |
| **Secondary** | ?? Azul Claro | `52, 152, 219` | Editar, Info |
| **Info** | ?? Púrpura | `155, 89, 182` | Funciones especiales |
| **Warning** | ?? Amarillo | `241, 196, 15` | Alertas |
| **Background** | ? Gris Claro | `236, 240, 245` | Fondo general |

---

## ?? PLANTILLA DE FORMULARIO LISTADO

```csharp
private void InitializeComponent()
{
    // Header Panel
    headerPanel = new Panel
    {
        BackColor = Color.FromArgb(41, 128, 185),
        Dock = DockStyle.Top,
        Size = new Size(1000, 80)
    };
    
    lblTitle = new Label
    {
        AutoSize = true,
        Font = new Font("Segoe UI", 24F, FontStyle.Bold),
        ForeColor = Color.White,
        Location = new Point(20, 20),
        Text = "Titulo del Formulario"
    };
    
    headerPanel.Controls.Add(lblTitle);
    
    // Content Panel
    contentPanel = new Panel
    {
        BackColor = Color.White,
        Dock = DockStyle.Fill,
        Padding = new Padding(20)
    };
    
    // DataGridView
    dataGridView = new DataGridView
    {
        AllowUserToAddRows = false,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        BackgroundColor = Color.White,
        BorderStyle = BorderStyle.None,
        ColumnHeadersHeight = 40,
        Dock = DockStyle.Fill,
        EnableHeadersVisualStyles = false,
        MultiSelect = false,
        ReadOnly = true,
        RowHeadersVisible = false,
        RowTemplate.Height = 35,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect
    };
    
    contentPanel.Controls.Add(dataGridView);
    
    // Button Panel
    buttonPanel = new Panel
    {
        BackColor = Color.FromArgb(236, 240, 245),
        Dock = DockStyle.Bottom,
        Size = new Size(1000, 70)
    };
    
    // Botones
    btnNuevo = new Button
    {
        BackColor = Color.FromArgb(46, 204, 113),
        FlatStyle = FlatStyle.Flat,
        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
        ForeColor = Color.White,
        Location = new Point(20, 15),
        Size = new Size(150, 40),
        Text = "Nuevo",
        Cursor = Cursors.Hand
    };
    btnNuevo.FlatAppearance.BorderSize = 0;
    
    buttonPanel.Controls.Add(btnNuevo);
    
    // Form
    this.ClientSize = new Size(1000, 670);
    this.Controls.Add(contentPanel);
    this.Controls.Add(buttonPanel);
    this.Controls.Add(headerPanel);
    this.FormBorderStyle = FormBorderStyle.FixedSingle;
    this.MaximizeBox = false;
    this.StartPosition = FormStartPosition.CenterScreen;
    this.WindowState = FormWindowState.Normal;
}
```

---

## ?? PLANTILLA DE FORMULARIO EDICIÓN

```csharp
private void InitializeComponent()
{
    // Header
    headerPanel = new Panel
    {
        BackColor = Color.FromArgb(41, 128, 185),
        Dock = DockStyle.Top,
        Size = new Size(550, 60)
    };
    
    lblTitle = new Label
    {
        AutoSize = true,
        Font = new Font("Segoe UI", 18F, FontStyle.Bold),
        ForeColor = Color.White,
        Location = new Point(20, 15),
        Text = "Editar Registro"
    };
    
    headerPanel.Controls.Add(lblTitle);
    
    // Content Panel con TableLayoutPanel
    contentPanel = new Panel
    {
        BackColor = Color.White,
        Dock = DockStyle.Fill,
        Padding = new Padding(20)
    };
    
    // Button Panel
    buttonPanel = new Panel
    {
        BackColor = Color.FromArgb(236, 240, 245),
        Dock = DockStyle.Bottom,
        Size = new Size(550, 70)
    };
    
    btnGuardar = new Button
    {
        BackColor = Color.FromArgb(46, 204, 113),
        FlatStyle = FlatStyle.Flat,
        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
        ForeColor = Color.White,
        Location = new Point(150, 15),
        Size = new Size(120, 40),
        Text = "Guardar",
        Cursor = Cursors.Hand
    };
    btnGuardar.FlatAppearance.BorderSize = 0;
    
    btnCancelar = new Button
    {
        BackColor = Color.FromArgb(231, 76, 60),
        FlatStyle = FlatStyle.Flat,
        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
        ForeColor = Color.White,
        Location = new Point(280, 15),
        Size = new Size(120, 40),
        Text = "Cancelar",
        Cursor = Cursors.Hand
    };
    btnCancelar.FlatAppearance.BorderSize = 0;
    
    buttonPanel.Controls.Add(btnGuardar);
    buttonPanel.Controls.Add(btnCancelar);
    
    // Form
    this.ClientSize = new Size(550, 400);
    this.Controls.Add(contentPanel);
    this.Controls.Add(buttonPanel);
    this.Controls.Add(headerPanel);
    this.FormBorderStyle = FormBorderStyle.FixedDialog;
    this.MaximizeBox = false;
    this.StartPosition = FormStartPosition.CenterParent;
}
```

---

## ?? TAMAÑOS ESTÁNDAR

### Formularios:
- **Listado CRUD**: 1000 x 670 px
- **Menú Principal**: 900 x 650 px
- **Edición Simple**: 550 x 400 px
- **Inscripciones**: 1200 x 700 px

### Componentes:
- **Header Panel**: Height = 80px (listado) o 60px (edición)
- **Button Panel**: Height = 70px
- **Botones**: 130-150px x 40px
- **Padding**: 20px

---

## ?? EFECTOS INTERACTIVOS

### Hover Effect para Botones:
```csharp
button.MouseEnter += (s, e) =>
{
    button.BackColor = ControlPaint.Light(originalColor, 0.2f);
};

button.MouseLeave += (s, e) =>
{
    button.BackColor = originalColor;
};
```

### Hover Effect para Cards:
```csharp
card.MouseEnter += (s, e) => card.BackColor = ControlPaint.Light(colorFondo, 0.2f);
card.MouseLeave += (s, e) => card.BackColor = colorFondo;
```

---

## ? CHECKLIST PARA NUEVOS FORMULARIOS

Cuando crees un nuevo formulario, asegúrate de:

- [ ] Aplicar `FormBorderStyle = FormBorderStyle.FixedSingle`
- [ ] Establecer `MaximizeBox = false`
- [ ] Configurar `WindowState = FormWindowState.Normal`
- [ ] Usar `StartPosition = FormStartPosition.CenterScreen`
- [ ] Agregar Header Panel con color Primary
- [ ] Usar colores semánticos en botones (Verde=Crear, Azul=Editar, Rojo=Eliminar)
- [ ] Aplicar `FlatStyle.Flat` a todos los botones
- [ ] Configurar `Cursor = Cursors.Hand` en botones
- [ ] Usar fuente Segoe UI
- [ ] Agregar padding de 20px en paneles de contenido
- [ ] Estilizar DataGridView si corresponde

---

## ?? SOLUCIÓN DE PROBLEMAS

### Problema: El formulario se maximiza al abrir
**Solución:**
```csharp
this.WindowState = FormWindowState.Normal;
this.MaximizeBox = false;
```

### Problema: Los botones no tienen el estilo correcto
**Solución:**
```csharp
button.FlatStyle = FlatStyle.Flat;
button.FlatAppearance.BorderSize = 0;
```

### Problema: El DataGridView no se ve moderno
**Solución:**
```csharp
FormStyles.StyleDataGridView(tuDataGridView);
```

---

**Última actualización**: 2025  
**Versión de estilos**: 2.0
