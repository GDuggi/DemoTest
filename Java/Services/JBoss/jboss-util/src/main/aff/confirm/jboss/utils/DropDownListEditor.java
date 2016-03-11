 package aff.confirm.jboss.utils;

 import java.awt.*;
 import java.beans.PropertyChangeEvent;
 import java.beans.PropertyChangeListener;
 import java.beans.PropertyEditor;
 import java.beans.PropertyEditorManager;
 import java.util.Vector;

 public class DropDownListEditor
   implements PropertyEditor
 {
   private DropDownList value;
   private Object source;
   private Vector listeners;

   public DropDownListEditor()
   {
     this.source = this;
   }

   public void setValue(Object value) {
     this.value = ((DropDownList)value);
     firePropertyChange();
   }

   public DropDownListEditor(Object source) {
     if (source == null)
       throw new NullPointerException();
     this.source = source;
   }

   public Object getValue() {
     return this.value;
   }

   public boolean isPaintable() {
     return false;
   }

   public void paintValue(Graphics gfx, Rectangle box) {
   }

   public String getJavaInitializationString() {
     return "???";
   }

   public String getAsText() {
     return this.value.getValue();
   }

   public void setAsText(String text) throws IllegalArgumentException {
     if (this.value != null)
       this.value.setValue(text);
   }

   public String[] getTags() {
     return this.value.getTags();
   }

   public Component getCustomEditor() {
     return null;
   }

   public boolean supportsCustomEditor() {
     return false;
   }

   public synchronized void addPropertyChangeListener(PropertyChangeListener listener)
   {
     if (this.listeners == null) {
       this.listeners = new Vector();
     }
     this.listeners.addElement(listener);
   }

   public synchronized void removePropertyChangeListener(PropertyChangeListener listener)
   {
     if (this.listeners == null) {
       return;
     }
     this.listeners.removeElement(listener);
   }

   public void firePropertyChange()
   {
     Vector targets;
     synchronized (this) {
       if (this.listeners == null) {
         return;
       }
       targets = (Vector)this.listeners.clone();
     }

     PropertyChangeEvent evt = new PropertyChangeEvent(this.source, null, null, null);

     for (int i = 0; i < targets.size(); i++) {
       PropertyChangeListener target = (PropertyChangeListener)targets.elementAt(i);
       target.propertyChange(evt);
     }
   }

   static {
     PropertyEditorManager.registerEditor(DropDownList.class, DropDownListEditor.class);
   }
 }

